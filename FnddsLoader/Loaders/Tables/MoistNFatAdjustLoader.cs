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
/// This class contains functionaility for loading data for the moisture and fat
/// adjustment table.
/// </summary>
public class MoistNFatAdjustLoader : DataLoader
{
    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<MoistNFatAdjustLoader> _logger =
        new NLogLoggerFactory().CreateLogger<MoistNFatAdjustLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new MoistNFatAdjustLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public MoistNFatAdjustLoader(FnddsVersion version, OleDbConnection connection, FnddsContext context)
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
                SourceName = "[Moisture change]",
                DestinationName = "MoistureChange",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Fat change]",
                DestinationName = "FatChange",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64 }
            },
            new DataColumnModel
            {
                SourceName = "[Type of fat]",
                DestinationName = "TypeOfFat",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64 }
            },
        };

    /// <inheritdoc />
    public override string TableName
    {
        get
        {
            return FnddsVersion.Id switch
            {
                1 or 2 or 4 or 8 or 16 or 32 or 64 => "MoistNFatAdjust",
                256 or 512 => "MoistAdjust",
                _ => string.Empty,
            };
        }
    }

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        var adjusts = new List<MoistNFatAdjust>();

        var recordCount = 0;
        while (reader.Read())
        {
            var adjust = new MoistNFatAdjust
            {
                Version = FnddsVersion.Id,
                Created = DateTime.Now
            };

            SetModelValues(columns, reader, adjust);

            adjusts.Add(adjust);

            if (_isDebugEnabled)
            {
                _logger.LogDebug("Table: {tableName}, Food code: {foodCode}", TableName, adjust.FoodCode);
            }

            if (adjusts.Count > BatchSize)
            {
                Context.MoistNFatAdjust.AddRange(adjusts);

                await Context.SaveChangesAsync();

                adjusts.Clear();
            }

            recordCount++;
        }

        if (adjusts.Count > 0)
        {
            Context.MoistNFatAdjust.AddRange(adjusts);

            await Context.SaveChangesAsync();
        }

        return recordCount;
    }
}
