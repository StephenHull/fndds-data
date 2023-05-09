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
/// This class contains functionaility for loading data for the derivation
/// description table.
/// </summary>
public class DerivDescLoader : DataLoader
{
    /// <summary>
    /// The table name in the source database.
    /// </summary>
    private const string SourceTableName = "DerivDesc";

    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<DerivDescLoader> _logger = new NLogLoggerFactory().CreateLogger<DerivDescLoader>();

    /// <summary>
    /// True if the logger is debug endabled; otherwise, false.
    /// </summary>
    private readonly bool _isDebugEnabled = false;

    /// <summary>
    /// Constructs a new DerivDescLoader object.
    /// </summary>
    /// <param name="version">The FNDDS version.</param>
    /// <param name="connection">The connection to the source database.</param>
    /// <param name="context">The destination database context.</param>
    public DerivDescLoader(FnddsVersion version, OleDbConnection connection, FnddsContext context)
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
                SourceName = "[Derivation code]",
                DestinationName = "DerivationCode",
                IsOrderedBy = true,
                Versions = new HashSet<int> { 256, 512 }
            },
            new DataColumnModel
            {
                SourceName = "[Derivation description]",
                DestinationName = "DerivationDescription",
                Versions = new HashSet<int> { 256, 512 }
            },
        };

    /// <inheritdoc />
    public override string TableName => SourceTableName;

    /// <inheritdoc />
    public override async Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader)
    {
        var derivations = new List<DerivDesc>();

        var recordCount = 0;
        while (reader.Read())
        {
            var derivation = new DerivDesc
            {
                Version = FnddsVersion.Id,
                Created = DateTime.Now
            };

            SetModelValues(columns, reader, derivation);

            derivations.Add(derivation);

            if (_isDebugEnabled)
            {
                _logger.LogDebug("Table: {tableName}, Derivation code: {derivationCode}", SourceTableName,
                    derivation.DerivationCode);
            }

            if (derivations.Count > BatchSize)
            {
                Context.DerivDesc.AddRange(derivations);

                await Context.SaveChangesAsync();

                derivations.Clear();
            }

            recordCount++;
        }

        Context.DerivDesc.AddRange(derivations);

        await Context.SaveChangesAsync();

        return recordCount;
    }
}
