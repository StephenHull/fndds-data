using System.Data.OleDb;
using FoodAndNutrientData.Base.Interfaces;
using FoodAndNutrientData.Importer.Contexts;
using FoodAndNutrientData.Importer.Entities;
using FoodAndNutrientData.Importer.Importers;
using FoodAndNutrientData.Importer.Importers.Tables;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FoodAndNutrientData.Importer;

public class Importer
{
    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<Importer> _logger = new NLogLoggerFactory().CreateLogger<Importer>();

    private readonly FnddsDbContext _dbContext;

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new FNDDS importer.
    /// </summary>
    public Importer(FnddsDbContext dbContext)
    {
        _dbContext = dbContext;
        _isDebugEnabled = _logger.IsEnabled(LogLevel.Debug);
    }

    /// <summary>
    /// Imports data from a source database.
    /// </summary>
    /// <param name="fnddsVersion">The FNDDS version.</param>
    /// <param name="fnddsConnString">The FNDDS connection string for the source database.</param>
    /// <param name="fpedConnString">The FPED connection string for the source database.</param>
    /// <param name="fpidConnString">The FPID connection string for the source database.</param>
    /// <returns>Returns true if the method completes successfully.</returns>
    public async Task<bool> ImportDataAsync(IFnddsVersion fnddsVersion, string fnddsConnString, string fpedConnString,
        string fpidConnString)
    {
        try
        {
            var version = _dbContext.FnddsVersions.SingleOrDefault(x => x.Id == fnddsVersion.Id);
            if (version != null)
            {
                _dbContext.FnddsVersions.Remove(version);

                await _dbContext.SaveChangesAsync();
            }

            version = new FnddsVersion
            {
                Id = fnddsVersion.Id,
                BeginYear = fnddsVersion.BeginYear,
                EndYear = fnddsVersion.EndYear,
                Major = fnddsVersion.Major,
                Minor = fnddsVersion.Minor,
                CreateDt = DateTime.Now
            };

            _dbContext.FnddsVersions.Add(version);

            await _dbContext.SaveChangesAsync();

            using (var connection = new OleDbConnection(fnddsConnString))
            {
                await connection.OpenAsync();

                var importers =
                    new List<DataImporter>
                    {
                        new DerivDescImporter(version, connection, _dbContext),
                        new FoodPortionDescImporter(version, connection, _dbContext),
                        new MainFoodDescImporter(version, connection, _dbContext),
                        new NutDescImporter(version, connection, _dbContext),
                        new SubcodeDescImporter(version, connection, _dbContext),

                        new AddFoodDescImporter(version, connection, _dbContext),
                        new FnddsIngredImporter(version, connection, _dbContext),
                        new FnddsNutValImporter(version, connection, _dbContext),
                        new FoodWeightImporter(version, connection, _dbContext),
                        new IngredNutValImporter(version, connection, _dbContext),
                        new ModDescImporter(version, connection, _dbContext),
                        new MoistNFatAdjustImporter(version, connection, _dbContext),

                        new ModNutValImporter(version, connection, _dbContext),
                    };

                foreach (var importer in importers)
                {
                    await importer.PrepareToImportAsync();

                    var recordsImported = await importer.ImportAsync();

                    if (_isDebugEnabled)
                    {
                        _logger.LogDebug("Table: {tableName}, Records: {recordCount}", importer.TableName, recordsImported);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(fpedConnString) && !string.IsNullOrWhiteSpace(fpidConnString))
            {
                using (var connection = new OleDbConnection(fpedConnString))
                {
                    await connection.OpenAsync();

                    var importers =
                        new List<DataImporter>
                        {
                            new FoodEquivImporter(version, connection, _dbContext),
                        };

                    foreach (var importer in importers)
                    {
                        await importer.PrepareToImportAsync();

                        var recordsImported = await importer.ImportAsync();

                        if (_isDebugEnabled)
                        {
                            _logger.LogDebug("Table: {tableName}, Records: {recordCount}", importer.TableName,
                                recordsImported);
                        }
                    }
                }

                using (var connection = new OleDbConnection(fpidConnString))
                {
                    await connection.OpenAsync();

                    var importers =
                        new List<DataImporter>
                        {
                            new IngredEquivImporter(version, connection, _dbContext),
                        };

                    foreach (var importer in importers)
                    {
                        await importer.PrepareToImportAsync();

                        var recordsImported = await importer.ImportAsync();

                        if (_isDebugEnabled)
                        {
                            _logger.LogDebug("Table: {tableName}, Records: {recordCount}", importer.TableName,
                                recordsImported);
                        }
                    }
                }
            }

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to import the data.");

            throw;
        }
    }
}
