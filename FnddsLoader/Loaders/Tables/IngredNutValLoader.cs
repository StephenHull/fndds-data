using System.Data.OleDb;
using FnddsData.Fndds.Models;
using FnddsData.FnddsLoader.Contexts;
using FnddsData.FnddsLoader.Entities;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FnddsData.FnddsLoader.Loaders.Tables;

/// <summary>
/// This class contains functionaility for loading data for the ingredient nutrient
/// values table.
/// </summary>
public class IngredNutValLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "IngredNutVal";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<IngredNutValLoader> _logger =
        new NLogLoggerFactory().CreateLogger<IngredNutValLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new IngredNutValLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public IngredNutValLoader(FnddsVersion version, OleDbConnection connection, FnddsDbContext context)
        : base(version, connection, context)
    {
        _isDebugEnabled = _logger.IsEnabled(LogLevel.Debug);
    }

    /// <inheritdoc />
    public override IEnumerable<DataColumnModel> Columns =>
        [
            new DataColumnModel
            {
                SourceName = "[Ingredient code]",
                DestinationName = "IngredientCode",
                IsOrderedBy = true,
                Versions = [128, 256, 512, 1024]
            },
            new DataColumnModel
            {
                SourceName = "[Nutrient code]",
                DestinationName = "NutrientCode",
                IsOrderedBy = true,
                Versions = [128, 256, 512, 1024]
            },
            new DataColumnModel
            {
                SourceName = "[Start date]",
                DestinationName = "StartDt",
                Versions = [128, 256]
            },
            new DataColumnModel
            {
                SourceName = "[End date]",
                DestinationName = "EndDt",
                Versions = [128, 256]
            },
            new DataColumnModel
            {
                SourceName = "[SR description]",
                DestinationName = "IngredientDescription",
                Versions = [128]
            },
            new DataColumnModel
            {
                SourceName = "[Ingredient description]",
                DestinationName = "IngredientDescription",
                Versions = [256, 512, 1024]
            },
            new DataColumnModel
            {
                SourceName = "[Nutrient value]",
                DestinationName = "NutrientValue",
                Versions = [128, 256, 512, 1024]
            },
            new DataColumnModel
            {
                SourceName = "[Nutrient value source]",
                DestinationName = "NutrientValueSource",
                Versions = [128, 256, 512, 1024]
            },
            new DataColumnModel
            {
                SourceName = "[FDC ID]",
                DestinationName = "FdcId",
                Versions = [256, 512, 1024]
            },
            new DataColumnModel
            {
                SourceName = "[SR 28 derivation code]",
                DestinationName = "DerivationCode",
                Versions = [128]
            },
            new DataColumnModel
            {
                SourceName = "[Derivation code]",
                DestinationName = "DerivationCode",
                Versions = [256, 512, 1024]
            },
            new DataColumnModel
            {
                SourceName = "[SR 28 AddMod year]",
                DestinationName = "SRAddModYear",
                Versions = [128]
            },
            new DataColumnModel
            {
                SourceName = "[SR AddMod year]",
                DestinationName = "SRAddModYear",
                Versions = [256, 512, 1024]
            },
            new DataColumnModel
            {
                SourceName = "[Foundation year acquired]",
                DestinationName = "FoundationYearAcquired",
                Versions = [256, 512, 1024]
            },
        ];

    public override string TableName => SourceTableName;

    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        try
        {
            var entities = new List<IngredNutVal>();

            var recordCount = 0;

            while (reader.Read())
            {
                var entity = new IngredNutVal
                {
                    VersionId = FnddsVersion.Id,
                    CreateDt = DateTime.UtcNow
                };

                SetModelValues(columns, reader, entity);

                entities.Add(entity);

                if (_isDebugEnabled)
                {
                    _logger.LogDebug("Table: {tableName}, Ingredient code: {ingredientCode}, Nutrient code: " +
                        "{nutrientCode}", SourceTableName, entity.IngredientCode, entity.NutrientCode);
                }

                if (entities.Count > BatchSize)
                {
                    Context.IngredNutVals.AddRange(entities);

                    await Context.SaveChangesAsync();

                    entities.Clear();
                }

                recordCount++;
            }

            if (entities.Count > 0)
            {
                Context.IngredNutVals.AddRange(entities);

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
