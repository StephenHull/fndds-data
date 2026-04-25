using System.Data.OleDb;
using FoodAndNutrientData.Base.Models;
using FoodAndNutrientData.Importer.Contexts;
using FoodAndNutrientData.Importer.Entities;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FoodAndNutrientData.Importer.Importers.Tables;

/// <summary>
/// This class contains functionaility for importing data for the subcode description
/// table.
/// </summary>
public class SubcodeDescImporter : DataImporter
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "SubcodeDesc";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<SubcodeDescImporter> _logger =
        new NLogLoggerFactory().CreateLogger<SubcodeDescImporter>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new SubcodeDescImporter object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public SubcodeDescImporter(FnddsVersion version, OleDbConnection connection, FnddsDbContext context)
        : base(version, connection, context)
    {
        _isDebugEnabled = _logger.IsEnabled(LogLevel.Debug);
    }

    /// <inheritdoc />
    public override IEnumerable<DataColumnModel> Columns =>
        [
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
                SourceName = "[Start date]",
                DestinationName = "StartDt",
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[End date]",
                DestinationName = "EndDt",
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Subcode description]",
                DestinationName = "SubcodeDescription",
                Versions =
                [
                    1, 2, 4, 8, 16, 32, 64, 128, 256,
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
            var entities = new List<SubcodeDesc>();

            var recordCount = 0;

            while (reader.Read())
            {
                var entity = new SubcodeDesc
                {
                    VersionId = FnddsVersion.Id,
                    CreateDt = DateTime.UtcNow
                };

                SetModelValues(columns, reader, entity);

                entities.Add(entity);

                if (_isDebugEnabled)
                {
                    _logger.LogDebug("Table: {tableName}, Subcode: {subcode}", SourceTableName, entity.Subcode);
                }

                if (entities.Count > BatchSize)
                {
                    Context.SubcodeDescs.AddRange(entities);

                    await Context.SaveChangesAsync();

                    entities.Clear();
                }

                recordCount++;
            }

            if (entities.Count > 0)
            {
                Context.SubcodeDescs.AddRange(entities);

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

    public override async Task<bool> PrepareToImportAsync()
    {
        try
        {
            var sql = FnddsVersion.Id switch
            {
                1 or 2 or 4 =>
                    "UPDATE SubcodeDesc " +
                    "SET [Subcode description] = 'Default Gram Weights' " +
                    "WHERE (Subcode = 0)",
                _ => string.Empty,
            };

            if (!string.IsNullOrWhiteSpace(sql))
            {
                using var command = new OleDbCommand(sql, Connection);

                await command.ExecuteNonQueryAsync();
            }

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to prepare to import the records for table {tableName}.", TableName);

            return false;
        }
    }
}
