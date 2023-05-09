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
/// This class contains functionaility for loading data for the main food
/// description table.
/// </summary>
public class MainFoodDescLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "MainFoodDesc";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<MainFoodDescLoader> _logger =
        new NLogLoggerFactory().CreateLogger<MainFoodDescLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new MainFoodDescLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public MainFoodDescLoader(FnddsVersion version, OleDbConnection connection, FnddsContext context)
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
                SourceName = "[Food code]",
                DestinationName = "FoodCode",
                IsOrderedBy = true,
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Start date]",
                DestinationName = "StartDate",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[End date]",
                DestinationName = "EndDate",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Main food description]",
                DestinationName = "MainFoodDescription",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Abbreviated description]",
                DestinationName = "AbbreviatedMainFoodDescription",
                Versions = new HashSet<int> { 1, 2, 4 }
            },
            new DataColumnModel
            {
                SourceName = "[Fortification identifier]",
                DestinationName = "FortificationIdentifier",
                Versions = new HashSet<int> { 32 }
            },
            new DataColumnModel
            {
                SourceName = "[Fortification identifier code]",
                DestinationName = "FortificationIdentifier",
                Versions = new HashSet<int> { 128 }
            },
            new DataColumnModel
            {
                SourceName = "[WWEIA Category code]",
                DestinationName = "CategoryNumber",
                Versions = new HashSet<int> { 128 }
            },
            new DataColumnModel
            {
                SourceName = "[WWEIA Category number]",
                DestinationName = "CategoryNumber",
                Versions = new HashSet<int> { 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[WWEIA Category description]",
                DestinationName = "CategoryDescription",
                Versions = new HashSet<int> { 256, 512 }
            },
        };

    /// <inheritdoc />
    public override string TableName => SourceTableName;

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        var foods = new List<MainFoodDesc>();

        var recordCount = 0;
        while (reader.Read())
        {
            var food = new MainFoodDesc
            {
                Version = FnddsVersion.Id,
                Created = DateTime.Now
            };

            SetModelValues(columns, reader, food);

            foods.Add(food);

            if (_isDebugEnabled)
            {
                _logger.LogDebug("Table: {tableName}, Food code: {foodCode}", SourceTableName, food.FoodCode);
            }

            if (foods.Count > BatchSize)
            {
                Context.MainFoodDesc.AddRange(foods);

                await Context.SaveChangesAsync();

                foods.Clear();
            }

            recordCount++;
        }

        if (foods.Count > 0)
        {
            Context.MainFoodDesc.AddRange(foods);

            await Context.SaveChangesAsync();
        }

        return recordCount;
    }
}
