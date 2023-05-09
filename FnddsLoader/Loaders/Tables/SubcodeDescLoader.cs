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
/// This class contains functionaility for loading data for the subcode description
/// table.
/// </summary>
public class SubcodeDescLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "SubcodeDesc";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<SubcodeDescLoader> _logger =
        new NLogLoggerFactory().CreateLogger<SubcodeDescLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new SubcodeDescLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public SubcodeDescLoader(FnddsVersion version, OleDbConnection connection, FnddsContext context)
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
                SourceName = "[Subcode]",
                DestinationName = "Subcode",
                IsOrderedBy = true,
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256 }
            },
            new DataColumnModel
            {
                SourceName = "[Start date]",
                DestinationName = "StartDate",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256 }
            },
            new DataColumnModel
            {
                SourceName = "[End date]",
                DestinationName = "EndDate",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256 }
            },
            new DataColumnModel
            {
                SourceName = "[Subcode description]",
                DestinationName = "SubcodeDescription",
                Versions = new HashSet<int> { 1, 2, 4, 8, 16, 32, 64, 256 }
            }
        };

    /// <inheritdoc />
    public override string TableName => SourceTableName;

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        var subcodes = new List<SubcodeDesc>();

        var recordCount = 0;
        while (reader.Read())
        {
            var subcode = new SubcodeDesc
            {
                Version = FnddsVersion.Id,
                Created = DateTime.Now
            };

            SetModelValues(columns, reader, subcode);

            subcodes.Add(subcode);

            if (_isDebugEnabled)
            {
                _logger.LogDebug("Table: {tableName}, Subcode: {subcode}", SourceTableName, subcode.Subcode);
            }

            if (subcodes.Count > BatchSize)
            {
                Context.SubcodeDesc.AddRange(subcodes);

                await Context.SaveChangesAsync();

                subcodes.Clear();
            }

            recordCount++;
        }

        if (subcodes.Count > 0)
        {
            Context.SubcodeDesc.AddRange(subcodes);

            await Context.SaveChangesAsync();
        }

        return recordCount;
    }
}
