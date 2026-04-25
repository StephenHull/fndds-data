using System.Data.OleDb;
using FoodAndNutrientData.Base.Models;
using FoodAndNutrientData.Importer.Contexts;
using FoodAndNutrientData.Importer.Entities;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FoodAndNutrientData.Importer.Importers.Tables;

/// <summary>
/// This class contains functionaility for importing data for the food equivalents 
/// table.
/// </summary>
public class FoodEquivImporter : DataImporter
{
    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<FoodEquivImporter> _logger =
        new NLogLoggerFactory().CreateLogger<FoodEquivImporter>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new FoodEquivImporter object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public FoodEquivImporter(FnddsVersion version, OleDbConnection connection, FnddsDbContext context)
        : base(version, connection, context)
    {
        _isDebugEnabled = _logger.IsEnabled(LogLevel.Debug);
    }

    /// <inheritdoc />
    public override IEnumerable<DataColumnModel> Columns =>
        [
            new DataColumnModel
            {
                SourceName = "FOODCODE",
                DestinationName = "FoodCode",
                IsOrderedBy = true,
                Versions =
                [
                    4, 8, 16, 32, 64, 128, 256,
                ],
            },
            new DataColumnModel
            {
                SourceName = "MODCODE",
                DestinationName = "ModCode",
                IsIgnored = true,
                IsOrderedBy = true,
                Versions =
                [
                    4, 8, 16, 32,
                ],
            },
            new DataColumnModel
            {
                SourceName = "DESCRIPTION",
                DestinationName = "FoodDescription",
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
                4 => "FPED_0506",
                8 => "FPED_0708",
                16 => "FPED_0910",
                32 => "FPED_1112",
                64 => "FPED_1314",
                128 => "FPED_1516",
                256 => "FPED_1718",
                _ => string.Empty,
            };
        }
    }

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        try
        {
            var entities = new List<FoodEquiv>();

            var recordCount = 0;

            while (reader.Read())
            {
                // Versions 4, 8, 16, and 32 have Mod Codes for the Equivalents, but NO Mod Codes for the Main Food
                // Descriptions.
                if (FnddsVersion.Id == 4 || FnddsVersion.Id == 8 || FnddsVersion.Id == 16 || FnddsVersion.Id == 32)
                {
                    var value = reader.GetValue(1);
                    if (int.TryParse(value.ToString(), out var modCode) == false)
                    {
                        var message = string.Format("Unable to parse mod code value {0}.", value);

                        throw new Exception(message);
                    }

                    if (modCode > 0)
                    {
                        continue;
                    }
                }

                var equivalent = new FoodEquiv
                {
                    VersionId = FnddsVersion.Id,
                    CreateDt = DateTime.Now
                };

                SetModelValues(columns, reader, equivalent);

                entities.Add(equivalent);

                if (_isDebugEnabled)
                {
                    _logger.LogDebug("Table: {0}, Food code: {1}", TableName, equivalent.FoodCode);
                }

                if (entities.Count > BatchSize)
                {
                    Context.FoodEquivs.AddRange(entities);

                    await Context.SaveChangesAsync();

                    entities.Clear();
                }

                recordCount++;
            }

            if (entities.Count > 0)
            {
                Context.FoodEquivs.AddRange(entities);

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
