using System.Data.OleDb;
using FoodAndNutrientData.Base.Models;
using FoodAndNutrientData.Importer.Contexts;
using FoodAndNutrientData.Importer.Entities;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FoodAndNutrientData.Importer.Loaders.Tables;

/// <summary>
/// This class contains functionaility for loading data for the ingredient equivalents 
/// table.
/// </summary>
public class IngredEquivLoader : DataLoader
{
    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<IngredEquivLoader> _logger =
        new NLogLoggerFactory().CreateLogger<IngredEquivLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new IngredEquivLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public IngredEquivLoader(FnddsVersion version, OleDbConnection connection, FnddsDbContext context)
        : base(version, connection, context)
    {
        _isDebugEnabled = _logger.IsEnabled(LogLevel.Debug);
    }

    /// <inheritdoc />
    public override IEnumerable<DataColumnModel> Columns =>
        [
            new DataColumnModel
            {
                SourceName = "CODE",
                DestinationName = "IngredientCode",
                IsOrderedBy = true,
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "DESCRIPTION",
                DestinationName = "IngredientDescription",
                IsOrderedBy = true,
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "F_TOTAL",
                DestinationName = "F_TOTAL",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "F_CITMLB",
                DestinationName = "F_CITMLB",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "F_OTHER",
                DestinationName = "F_OTHER",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "F_JUICE",
                DestinationName = "F_JUICE",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "V_TOTAL",
                DestinationName = "V_TOTAL",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "V_DRKGR",
                DestinationName = "V_DRKGR",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "V_REDOR_TOTAL",
                DestinationName = "V_REDOR_TOTAL",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "V_REDOR_TOMATO",
                DestinationName = "V_REDOR_TOMATO",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "V_REDOR_OTHER",
                DestinationName = "V_REDOR_OTHER",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "V_STARCHY_TOTAL",
                DestinationName = "V_STARCHY_TOTAL",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "V_STARCHY_POTATO",
                DestinationName = "V_STARCHY_POTATO",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "V_STARCHY_OTHER",
                DestinationName = "V_STARCHY_OTHER",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "V_OTHER",
                DestinationName = "V_OTHER",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "V_LEGUMES",
                DestinationName = "V_LEGUMES",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "G_TOTAL",
                DestinationName = "G_TOTAL",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "G_WHOLE",
                DestinationName = "G_WHOLE",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "G_REFINED",
                DestinationName = "G_REFINED",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_TOTAL",
                DestinationName = "PF_TOTAL",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_MPS_TOTAL",
                DestinationName = "PF_MPS_TOTAL",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_MEAT",
                DestinationName = "PF_MEAT",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_CUREDMEAT",
                DestinationName = "PF_CUREDMEAT",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_ORGAN",
                DestinationName = "PF_ORGAN",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_POULT",
                DestinationName = "PF_POULT",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_SEAFD_HI",
                DestinationName = "PF_SEAFD_HI",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_SEAFD_LOW",
                DestinationName = "PF_SEAFD_LOW",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_EGGS",
                DestinationName = "PF_EGGS",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_SOY",
                DestinationName = "PF_SOY",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_NUTSDS",
                DestinationName = "PF_NUTSDS",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "PF_LEGUMES",
                DestinationName = "PF_LEGUMES",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "D_TOTAL",
                DestinationName = "D_TOTAL",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "D_MILK",
                DestinationName = "D_MILK",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "D_YOGURT",
                DestinationName = "D_YOGURT",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "D_CHEESE",
                DestinationName = "D_CHEESE",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "OILS",
                DestinationName = "OILS",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "SOLID_FATS",
                DestinationName = "SOLID_FATS",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "ADD_SUGARS",
                DestinationName = "ADD_SUGARS",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "A_DRINKS",
                DestinationName = "A_DRINKS",
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
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
                4 => "FPID_0506",
                8 => "FPID_0708",
                16 => "FPID_0910",
                32 => "FPID_1112",
                64 => "FPID_1314",
                128 => "FPID_1516",
                256 => "FPID_1718",
                _ => string.Empty,
            };
        }
    }

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        try
        {
            var entities = new List<IngredEquiv>();

            var recordCount = 0;

            while (reader.Read())
            {
                var equivalent = new IngredEquiv
                {
                    VersionId = FnddsVersion.Id,
                    CreateDt = DateTime.Now
                };

                SetModelValues(columns, reader, equivalent);

                entities.Add(equivalent);

                if (_isDebugEnabled)
                {
                    _logger.LogDebug("Table: {0}, Ingredient code: {1}", TableName, equivalent.IngredientCode);
                }

                if (entities.Count > BatchSize)
                {
                    Context.IngredEquivs.AddRange(entities);

                    await Context.SaveChangesAsync();

                    entities.Clear();
                }

                recordCount++;
            }

            if (entities.Count > 0)
            {
                Context.IngredEquivs.AddRange(entities);

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
