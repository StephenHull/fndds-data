using System.Data.OleDb;
using FnddsData.Fndds.Models;
using FnddsData.FnddsLoader.Contexts;
using FnddsData.FnddsLoader.Entities;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FnddsData.FnddsLoader.Loaders.Tables;

/// <summary>
/// This class contains functionaility for loading data for the food portion
/// description table.
/// </summary>
public class FoodPortionDescLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "FoodPortionDesc";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<FoodPortionDescLoader> _logger =
        new NLogLoggerFactory().CreateLogger<FoodPortionDescLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new FoodPortionDescLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public FoodPortionDescLoader(FnddsVersion version, OleDbConnection connection, FnddsDbContext context)
        : base(version, connection, context)
    {
        _isDebugEnabled = _logger.IsEnabled(LogLevel.Debug);
    }

    /// <inheritdoc />
    public override IEnumerable<DataColumnModel> Columns =>
        [
            new DataColumnModel
            {
                SourceName = "[Portion code]",
                DestinationName = "PortionCode",
                IsOrderedBy = true,
                Versions = [1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024]
            },
            new DataColumnModel
            {
                SourceName = "[Start date]",
                DestinationName = "StartDt",
                Versions = [1, 2, 4, 8, 16, 32, 64, 128, 256]
            },
            new DataColumnModel
            {
                SourceName = "[End date]",
                DestinationName = "EndDt",
                Versions = [1, 2, 4, 8, 16, 32, 64, 128, 256]
            },
            new DataColumnModel
            {
                SourceName = "[Portion description]",
                DestinationName = "PortionDescription",
                Versions = [1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024]
            },
            new DataColumnModel
            {
                SourceName = "[Change type]",
                DestinationName = "ChangeType",
                Versions = [1, 2, 4, 8, 16, 32]
            },
        ];

    /// <inheritdoc />
    public override string TableName => SourceTableName;

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        try
        {
            var entities = new List<FoodPortionDesc>();

            var recordCount = 0;

            while (reader.Read())
            {
                var entity = new FoodPortionDesc
                {
                    VersionId = FnddsVersion.Id,
                    CreateDt = DateTime.UtcNow
                };

                SetModelValues(columns, reader, entity);

                entities.Add(entity);

                if (_isDebugEnabled)
                {
                    _logger.LogDebug("Table: {tableName}, Portion code: {portionCode}", SourceTableName,
                        entity.PortionCode);
                }

                if (entities.Count > BatchSize)
                {
                    Context.FoodPortionDescs.AddRange(entities);

                    await Context.SaveChangesAsync();

                    entities.Clear();
                }

                recordCount++;
            }

            if (entities.Count > 0)
            {
                Context.FoodPortionDescs.AddRange(entities);

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
