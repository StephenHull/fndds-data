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
/// This class contains functionaility for loading data for the additional food
/// description table.
/// </summary>
public class AddFoodDescLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "AddFoodDesc";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<AddFoodDescLoader> _logger =
        new NLogLoggerFactory().CreateLogger<AddFoodDescLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new AddFoodDescLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public AddFoodDescLoader(FnddsVersion version, OleDbConnection connection, FnddsContext context)
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
                SourceName = "[Seq num]",
                DestinationName = "SeqNum",
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
                SourceName = "[Additional food description]",
                DestinationName = "AdditionalFoodDescription",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 }
            },
        };

    /// <inheritdoc />
    public override string TableName => SourceTableName;

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        var descriptions = new List<AddFoodDesc>();

        var recordCount = 0;
        while (reader.Read())
        {
            var description = new AddFoodDesc
            {
                Version = FnddsVersion.Id,
                Created = DateTime.Now
            };

            SetModelValues(columns, reader, description);

            descriptions.Add(description);

            if (_isDebugEnabled)
            {
                _logger.LogDebug("Table: {tableName}, Food code: {foodCode}, Sequence: {sequenceNumber}",
                    SourceTableName, description.FoodCode, description.SeqNum);
            }

            if (descriptions.Count > BatchSize)
            {
                Context.AddFoodDesc.AddRange(descriptions);

                await Context.SaveChangesAsync();

                descriptions.Clear();
            }

            recordCount++;
        }

        if (descriptions.Count > 0)
        {
            Context.AddFoodDesc.AddRange(descriptions);

            await Context.SaveChangesAsync();
        }

        return recordCount;
    }
}
