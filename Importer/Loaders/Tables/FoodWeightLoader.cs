using System.Data.OleDb;
using FoodAndNutrientData.Base.Models;
using FoodAndNutrientData.Importer.Contexts;
using FoodAndNutrientData.Importer.Entities;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FoodAndNutrientData.Importer.Loaders.Tables;

/// <summary>
/// This class contains functionaility for loading data for the food weights table.
/// </summary>
public class FoodWeightLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "FoodWeights";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<FoodWeightLoader> _logger =
        new NLogLoggerFactory().CreateLogger<FoodWeightLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new FoodWeightsLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public FoodWeightLoader(FnddsVersion version, OleDbConnection connection, FnddsDbContext context)
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
                SourceName = "[Subcode]",
                DestinationName = "Subcode",
                IsOrderedBy = true,
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Seq num]",
                DestinationName = "SeqNum",
                IsOrderedBy = true,
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Portion code]",
                DestinationName = "PortionCode",
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
                SourceName = "[Portion weight]",
                DestinationName = "PortionWeight",
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Change type]",
                DestinationName = "ChangeType",
                Versions =
                [
                    1, 2, 4, 8, 16, 32,
                ],
            },
        ];

    /// <inheritdoc />
    public override string TableName => SourceTableName;

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        try
        {
            var entities = new List<FoodWeight>();

            var recordCount = 0;

            while (reader.Read())
            {
                var entity = new FoodWeight
                {
                    VersionId = FnddsVersion.Id,
                    CreateDt = DateTime.UtcNow
                };

                SetModelValues(columns, reader, entity);

                entities.Add(entity);

                if (_isDebugEnabled)
                {
                    _logger.LogDebug("Table: {tableName}, Food code: {foodCode}, Subcode: {subcode}, Sequence: " +
                        "{sequence}, Portion code: {portionCode}", SourceTableName, entity.FoodCode, entity.Subcode,
                        entity.SeqNum, entity.PortionCode);
                }

                if (entities.Count > BatchSize)
                {
                    Context.FoodWeights.AddRange(entities);

                    await Context.SaveChangesAsync();

                    entities.Clear();
                }

                recordCount++;
            }

            if (entities.Count > 0)
            {
                Context.FoodWeights.AddRange(entities);

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
