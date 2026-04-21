using System.Data.OleDb;
using FoodAndNutrientData.Base.Models;
using FoodAndNutrientData.Importer.Contexts;
using FoodAndNutrientData.Importer.Entities;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FoodAndNutrientData.Importer.Loaders.Tables;

/// <summary>
/// This class contains functionaility for loading data for the derivation
/// description table.
/// </summary>
public class DerivDescLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "DerivDesc";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<DerivDescLoader> _logger = new NLogLoggerFactory().CreateLogger<DerivDescLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new DerivDescLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public DerivDescLoader(FnddsVersion version, OleDbConnection connection, FnddsDbContext context)
        : base(version, connection, context)
    {
        _isDebugEnabled = _logger.IsEnabled(LogLevel.Debug);
    }

    /// <inheritdoc />
    public override IEnumerable<DataColumnModel> Columns =>
        [
            new DataColumnModel
            {
                SourceName = "[SR 28 derivation code]",
                DestinationName = "DerivationCode",
                IsOrderedBy = true,
                Versions =
                [
                    128,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Derivation code]",
                DestinationName = "DerivationCode",
                IsOrderedBy = true,
                Versions =
                [
                    256, 512, 1024,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[SR 28 derivation description]",
                DestinationName = "DerivationDescription",
                Versions =
                [
                    128,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Derivation description]",
                DestinationName = "DerivationDescription",
                Versions =
                [
                    256, 512, 1024,
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
            var entities = new List<DerivDesc>();

            var recordCount = 0;

            while (reader.Read())
            {
                var entity = new DerivDesc
                {
                    VersionId = FnddsVersion.Id,
                    CreateDt = DateTime.UtcNow
                };

                SetModelValues(columns, reader, entity);

                entities.Add(entity);

                if (_isDebugEnabled)
                {
                    _logger.LogDebug("Table: {tableName}, Derivation code: {derivationCode}", SourceTableName,
                        entity.DerivationCode);
                }

                if (entities.Count > BatchSize)
                {
                    Context.DerivDescs.AddRange(entities);

                    await Context.SaveChangesAsync();

                    entities.Clear();
                }

                recordCount++;
            }

            Context.DerivDescs.AddRange(entities);

            await Context.SaveChangesAsync();

            return recordCount;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create the records for table {tableName}.", TableName);

            throw;
        }
    }
}
