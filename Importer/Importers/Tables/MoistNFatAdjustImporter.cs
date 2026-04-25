using System.Data.OleDb;
using FoodAndNutrientData.Base.Models;
using FoodAndNutrientData.Importer.Contexts;
using FoodAndNutrientData.Importer.Entities;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FoodAndNutrientData.Importer.Importers.Tables;

/// <summary>
/// This class contains functionaility for importing data for the moisture and fat
/// adjustment table.
/// </summary>
public class MoistNFatAdjustImporter : DataImporter
{
    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<MoistNFatAdjustImporter> _logger =
        new NLogLoggerFactory().CreateLogger<MoistNFatAdjustImporter>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new MoistNFatAdjustImporter object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public MoistNFatAdjustImporter(FnddsVersion version, OleDbConnection connection, FnddsDbContext context)
        : base(version, connection, context)
    {
        _isDebugEnabled = _logger.IsEnabled(LogLevel.Debug);
    }

    /// <inheritdoc />
    public override IEnumerable<DataColumnModel> Columns =>
        [
            new DataColumnModel
            {
                SourceName = "[Food code]",
                DestinationName = "FoodCode",
                IsOrderedBy = true,
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Start date]",
                DestinationName = "StartDt",
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[End date]",
                DestinationName = "EndDt",
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Moisture change]",
                DestinationName = "MoistureChange",
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Fat change]",
                DestinationName = "FatChange",
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Type of fat]",
                DestinationName = "TypeOfFat",
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64,
                ],
            },
        ];

    /// <inheritdoc />
    public override string TableName
    {
        get
        {
            return FnddsVersion.Id switch
            {
                1 or 2 or 4 or 8 or 16 or 32 or 64 => "MoistNFatAdjust",
                128 or 256 or 512 or 1024 => "MoistAdjust",
                _ => string.Empty,
            };
        }
    }

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        try
        {
            var entities = new List<MoistNFatAdjust>();

            var recordCount = 0;

            while (reader.Read())
            {
                var entity = new MoistNFatAdjust
                {
                    VersionId = FnddsVersion.Id,
                    CreateDt = DateTime.UtcNow
                };

                SetModelValues(columns, reader, entity);

                entities.Add(entity);

                if (_isDebugEnabled)
                {
                    _logger.LogDebug("Table: {tableName}, Food code: {foodCode}", TableName, entity.FoodCode);
                }

                if (entities.Count > BatchSize)
                {
                    Context.MoistNFatAdjusts.AddRange(entities);

                    await Context.SaveChangesAsync();

                    entities.Clear();
                }

                recordCount++;
            }

            if (entities.Count > 0)
            {
                Context.MoistNFatAdjusts.AddRange(entities);

                await Context.SaveChangesAsync();
            }

            return recordCount;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create the records for table {tableName}.", TableName);

            throw;
        }
    }
}
