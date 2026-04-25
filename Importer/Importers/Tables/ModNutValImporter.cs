using System.Data.OleDb;
using FoodAndNutrientData.Base.Models;
using FoodAndNutrientData.Importer.Contexts;
using FoodAndNutrientData.Importer.Entities;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FoodAndNutrientData.Importer.Importers.Tables;

/// <summary>
/// This class contains functionaility for importing data for the food modification
/// nutrient values table.
/// </summary>
public class ModNutValImporter : DataImporter
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    public static string SourceTableName = "ModNutVal";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<ModNutValImporter> _logger =
        new NLogLoggerFactory().CreateLogger<ModNutValImporter>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new ModNutValImporter object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public ModNutValImporter(FnddsVersion version, OleDbConnection connection, FnddsDbContext context)
        : base(version, connection, context)
    {
        _isDebugEnabled = _logger.IsEnabled(LogLevel.Debug);
    }

    /// <inheritdoc />
    public override IEnumerable<DataColumnModel> Columns =>
        [
            new DataColumnModel
            {
                SourceName = "[Modification code]",
                DestinationName = "ModificationCode",
                IsOrderedBy = true,
                Versions =
                [
                    16, 32,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Nutrient code]",
                DestinationName = "NutrientCode",
                IsOrderedBy = true,
                Versions =
                [
                    16, 32,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Start date]",
                DestinationName = "StartDt",
                Versions =
                [
                    16, 32,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[End date]",
                DestinationName = "EndDt",
                Versions =
                [
                    16, 32,
                ],
            },
            new DataColumnModel
            {
                SourceName = "[Nutrient value]",
                DestinationName = "NutrientValue",
                Versions =
                [
                    16, 32,
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
            var nutrients = new List<ModNutVal>();

            var recordCount = 0;
            while (reader.Read())
            {
                var nutrient = new ModNutVal
                {
                    VersionId = FnddsVersion.Id,
                    CreateDt = DateTime.UtcNow
                };

                SetModelValues(columns, reader, nutrient);

                nutrients.Add(nutrient);

                if (_isDebugEnabled)
                {
                    _logger.LogDebug("Table: {0}, Modification code: {1}, Nutrient code: {2}",
                        SourceTableName, nutrient.ModificationCode, nutrient.NutrientCode);
                }

                if (nutrients.Count > BatchSize)
                {
                    Context.ModNutVals.AddRange(nutrients);

                    await Context.SaveChangesAsync();

                    nutrients.Clear();
                }

                recordCount++;
            }

            if (nutrients.Count > 0)
            {
                Context.ModNutVals.AddRange(nutrients);

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
