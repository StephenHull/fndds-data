using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;
using Fndds.Models;
using FnddsLoader.Data;
using FnddsLoader.Data.Models;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FnddsLoader.Loaders.Tables;

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
    public IngredNutValLoader(FnddsVersion version, OleDbConnection connection, FnddsContext context)
        : base(version, connection, context)
    {
        _isDebugEnabled = _logger.IsEnabled(LogLevel.Debug);
    }

    /// <inheritdoc />
    public override IEnumerable<DataColumnModel> Columns =>
        new List<DataColumnModel>
        {
            new DataColumnModel
            {
                SourceName = "[Ingredient code]",
                DestinationName = "IngredientCode",
                IsOrderedBy = true,
                Versions = new HashSet<int> { 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Nutrient code]",
                DestinationName = "NutrientCode",
                IsOrderedBy = true,
                Versions = new HashSet<int> { 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Start date]",
                DestinationName = "StartDate",
                Versions = new HashSet<int> { 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[End date]",
                DestinationName = "EndDate",
                Versions = new HashSet<int> { 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[SR description]",
                DestinationName = "IngredientDescription",
                Versions = new HashSet<int> { 128 }
            },
            new DataColumnModel
            {
                SourceName = "[Ingredient description]",
                DestinationName = "IngredientDescription",
                Versions = new HashSet<int> { 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Nutrient value]",
                DestinationName = "NutrientValue",
                Versions = new HashSet<int> { 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Nutrient value source]",
                DestinationName = "NutrientValueSource",
                Versions = new HashSet<int> { 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[FDC ID]",
                DestinationName = "FdcId",
                Versions = new HashSet<int> { 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[SR 28 derivation code]",
                DestinationName = "DerivationCode",
                Versions = new HashSet<int> { 128 }
            },
            new DataColumnModel
            {
                SourceName = "[Derivation code]",
                DestinationName = "DerivationCode",
                Versions = new HashSet<int> { 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[SR 28 AddMod year]",
                DestinationName = "SrAddModYear",
                Versions = new HashSet<int> { 128 }
            },
            new DataColumnModel
            {
                SourceName = "[SR AddMod year]",
                DestinationName = "SrAddModYear",
                Versions = new HashSet<int> { 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Foundation year acquired]",
                DestinationName = "FoundationYearAcquired",
                Versions = new HashSet<int> { 256, 512 }
            },
        };

    public override string TableName => SourceTableName;

    public async override Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        var nutrients = new List<IngredNutVal>();

        var recordCount = 0;
        while (reader.Read())
        {
            var nutrient = new IngredNutVal
            {
                Version = FnddsVersion.Id,
                Created = DateTime.Now
            };

            SetModelValues(columns, reader, nutrient);

            nutrients.Add(nutrient);

            if (_isDebugEnabled)
            {
                _logger.LogDebug("Table: {tableName}, Ingredient code: {ingredientCode}, Nutrient code: {nutrientCode}",
                    SourceTableName, nutrient.IngredientCode, nutrient.NutrientCode);
            }

            if (nutrients.Count > BatchSize)
            {
                Context.IngredNutVal.AddRange(nutrients);

                await Context.SaveChangesAsync();

                nutrients.Clear();
            }

            recordCount++;
        }

        if (nutrients.Count > 0)
        {
            Context.IngredNutVal.AddRange(nutrients);

            await Context.SaveChangesAsync();
        }

        return recordCount;
    }
}
