using System.Data.OleDb;
using FnddsData.Fndds.Extensions;
using FnddsData.Fndds.Interfaces;
using FnddsData.Fndds.Models;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace FnddsData.Fndds.Services;

/// <summary>
/// The class contains functionality for loading data from the source database into
/// a destination database table.
/// </summary>
public abstract class DataLoaderBase
{
    /// <summary>
    /// The logger class.
    /// </summary>
    private static readonly ILogger<DataLoaderBase> _logger = new NLogLoggerFactory().CreateLogger<DataLoaderBase>();

    /// <summary>
    /// The default batch size for the data loader.
    /// </summary>
    public const int BatchSize = 10_000; // TODO Make this configurable

    /// <summary>
    /// Gets the list of database column descriptions.
    /// </summary>
    public abstract IEnumerable<DataColumnModel> Columns { get; }

    /// <summary>
    /// The source database connection.
    /// </summary>
    public abstract OleDbConnection Connection { get; set; }

    /// <summary>
    /// The FNDDS version.
    /// </summary>
    public abstract IFnddsVersion FnddsVersion { get; set; }

    /// <summary>
    /// Creates records in the destination database based on data from the source
    /// database.
    /// </summary>
    /// <param name="columns">The database column descriptions.</param>
    /// <param name="reader">The source database data reader.</param>
    /// <returns>The number of records created.</returns>
    public abstract Task<int> CreateRecordsAsync(IEnumerable<DataColumnModel> columns, OleDbDataReader reader);

    /// <summary>
    /// Gets the source database table name.
    /// </summary>
    public abstract string TableName { get; }

    /// <summary>
    /// Load the data from the source database into the destination database.
    /// </summary>
    /// <returns>The number of records created.</returns>
    public async Task<int> LoadAsync()
    {
        try
        {
            var columns = new List<DataColumnModel>();

            foreach (var column in Columns)
            {
                if (column.Versions.Contains(FnddsVersion.Id))
                {
                    columns.Add(column);
                }
            }

            var recordCount = 0;

            if (columns.Count > 0)
            {
                var sql = columns.GetSql(TableName);

                _logger.LogDebug("SQL for table {tableName}: {sql}", TableName, sql);

                try
                {
                    using var command = new OleDbCommand(sql, Connection);
                    using var reader = command.ExecuteReader();

                    recordCount = await CreateRecordsAsync(columns, reader);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to create the records for table {tableName}.", TableName);

                    throw;
                }
            }

            return recordCount;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create the records for table {tableName}.", TableName);

            throw;
        }
    }

    /// <summary>
    /// Sets the model values using the data from the source database.
    /// </summary>
    /// <param name="columns">The database column descriptions.</param>
    /// <param name="reader">The source database data reader.</param>
    /// <param name="model">The data model.</param>
    public static void SetModelValues(IEnumerable<DataColumnModel> columns, OleDbDataReader reader, object model)
    {
        var type = model.GetType();

        try
        {
            var index = 0;

            foreach (var column in columns)
            {
                if (column.IsIgnored)
                {
                    continue;
                }

                var value = reader.GetValue(index++);
                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                {
                    var property = type.GetProperty(column.DestinationName);
                    if (property == null)
                    {
                        throw new Exception($"Unable to get properties for column {column.DestinationName}.");
                    }

                    var propertyType = property.PropertyType;

                    if (propertyType == typeof(DateTime))
                    {
                        if (DateTime.TryParse(value.ToString(), out var dateTimeValue) == false)
                        {
                            throw new Exception($"Unable to parse date/time value {value} for column {column.SourceName}.");
                        }

                        property.SetValue(model, dateTimeValue);
                    }
                    else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                    {
                        if (decimal.TryParse(value.ToString(), out var decimalValue) == false)
                        {
                            throw new Exception($"Unable to parse decimal value {value} for column {column.SourceName}.");
                        }

                        property.SetValue(model, decimalValue);
                    }
                    else if (propertyType == typeof(int) || propertyType == typeof(int?))
                    {
                        if (int.TryParse(value.ToString(), out var intValue) == false)
                        {
                            throw new Exception($"Unable to parse integer value for column {column.SourceName}.");
                        }

                        property.SetValue(model, intValue);
                    }
                    else if (propertyType == typeof(string))
                    {
                        var strValue = value.ToString();
                        if (string.IsNullOrEmpty(strValue) == false)
                        {
                            property.SetValue(model, strValue);
                        }
                    }
                    else
                    {
                        property.SetValue(model, value);
                    }
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to set the values for model {typeName}.", type.Name);

            throw;
        }
    }
}
