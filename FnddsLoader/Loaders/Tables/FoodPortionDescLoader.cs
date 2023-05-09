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
/// This class contains functionaility for loading data for the food portion
/// description table.
/// </summary>
public class FoodPortionDescLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "FoodPortionDesc";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<FoodPortionDescLoader> _logger =
        new NLogLoggerFactory().CreateLogger<FoodPortionDescLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new FoodPortionDescLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public FoodPortionDescLoader(FnddsVersion version, OleDbConnection connection, FnddsContext context)
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
                SourceName = "[Portion code]",
                DestinationName = "PortionCode",
                IsOrderedBy = true,
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Start date]",
                DestinationName = "StartDate",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[End date]",
                DestinationName = "EndDate",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Portion description]",
                DestinationName = "PortionDescription",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Change type]",
                DestinationName = "ChangeType",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32 }
            },
        };

    /// <inheritdoc />
    public override string TableName => SourceTableName;

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        var portions = new List<FoodPortionDesc>();

        var recordCount = 0;
        while (reader.Read())
        {
            var portion = new FoodPortionDesc
            {
                Version = FnddsVersion.Id,
                Created = DateTime.Now
            };

            SetModelValues(columns, reader, portion);

            portions.Add(portion);

            if (_isDebugEnabled)
            {
                _logger.LogDebug("Table: {tableName}, Portion code: {portionCode}", SourceTableName,
                    portion.PortionCode);
            }

            if (portions.Count > BatchSize)
            {
                Context.FoodPortionDesc.AddRange(portions);

                await Context.SaveChangesAsync();

                portions.Clear();
            }

            recordCount++;
        }

        if (portions.Count > 0)
        {
            Context.FoodPortionDesc.AddRange(portions);

            await Context.SaveChangesAsync();
        }

        return recordCount;
    }
}
