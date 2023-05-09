using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;
using Fndds.Models;
using FnddsLoader.Data;
using FnddsLoader.Data.Models;
using FnddsLoader.Loaders;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FnddsLoader.Loader.Tables;

/// <summary>
/// This class contains functionaility for loading data for the nutrient
/// description table.
/// </summary>
public class NutDescLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "NutDesc";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<NutDescLoader> _logger = new NLogLoggerFactory().CreateLogger<NutDescLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new NutDescLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public NutDescLoader(FnddsVersion version, OleDbConnection connection, FnddsContext context)
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
                SourceName = "[Nutrient code]",
                DestinationName = "NutrientCode",
                IsOrderedBy = true,
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Nutrient description]",
                DestinationName = "NutrientDescription",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "Tagname",
                DestinationName = "Tagname",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "Unit",
                DestinationName = "Unit",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "Decimals",
                DestinationName = "Decimals",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }
            },
        };

    /// <inheritdoc />
    public override string TableName => SourceTableName;

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        var nutrients = new List<NutDesc>();

        var recordCount = 0;
        while (reader.Read())
        {
            var nutrient = new NutDesc
            {
                Version = FnddsVersion.Id,
                Created = DateTime.Now
            };

            SetModelValues(columns, reader, nutrient);

            nutrients.Add(nutrient);

            if (_isDebugEnabled)
            {
                _logger.LogDebug("Table: {tableName}, Nutrient code: {nutrientCode}", SourceTableName,
                    nutrient.NutrientCode);
            }

            if (nutrients.Count > BatchSize)
            {
                Context.NutDesc.AddRange(nutrients);

                await Context.SaveChangesAsync();

                nutrients.Clear();
            }

            recordCount++;
        }

        Context.NutDesc.AddRange(nutrients);

        await Context.SaveChangesAsync();

        return recordCount;
    }
}
