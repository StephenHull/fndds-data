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
/// This class contains functionaility for loading data for the food weights table.
/// </summary>
public class FoodWeightsLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "FoodWeights";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<FoodWeightsLoader> _logger =
        new NLogLoggerFactory().CreateLogger<FoodWeightsLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new FoodWeightsLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public FoodWeightsLoader(FnddsVersion version, OleDbConnection connection, FnddsContext context)
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
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Subcode]",
                DestinationName = "Subcode",
                IsOrderedBy = true,
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256 }
            },
            new DataColumnModel
            {
                SourceName = "[Seq num]",
                DestinationName = "SeqNum",
                IsOrderedBy = true,
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256, 512 }
            },
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
                SourceName = "[Portion weight]",
                DestinationName = "PortionWeight",
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
        var weights = new List<FoodWeights>();

        var recordCount = 0;
        while (reader.Read())
        {
            var weight = new FoodWeights
            {
                Version = FnddsVersion.Id,
                Created = DateTime.Now
            };

            SetModelValues(columns, reader, weight);

            weights.Add(weight);

            if (_isDebugEnabled)
            {
                _logger.LogDebug("Table: {tableName}, Food code: {foodCode}, Subcode: {subcode}, " +
                    "Sequence: {sequence}, Portion code: {portionCode}", SourceTableName, weight.FoodCode,
                    weight.Subcode, weight.SeqNum, weight.PortionCode);
            }

            if (weights.Count > BatchSize)
            {
                Context.FoodWeights.AddRange(weights);

                await Context.SaveChangesAsync();

                weights.Clear();
            }

            recordCount++;
        }

        if (weights.Count > 0)
        {
            Context.FoodWeights.AddRange(weights);

            await Context.SaveChangesAsync();
        }

        return recordCount;
    }
}
