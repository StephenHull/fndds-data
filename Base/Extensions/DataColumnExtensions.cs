using System.Text;
using FnddsData.Fndds.Models;

namespace FnddsData.Fndds.Extensions;

/// <summary>
/// This class contains utility methods for building SQL statements.
/// </summary>
public static class DataColumnExtensions
{
    /// <summary>
    /// Returns a SQL statement for retrieving data from the source database.
    /// </summary>
    /// <param name="columns">The list of database columns.</param>
    /// <param name="tableName">The table name.</param>
    /// <returns>A SQL startment with a SELECT clause and an optional ORDER BY clause.</returns>
    public static string GetSql(this IEnumerable<DataColumnModel> columns, string tableName)
    {

        var selectColumns = new StringBuilder();
        var orderByColumns = new StringBuilder();

        foreach (var column in columns)
        {
            if (selectColumns.Length > 0)
            {
                selectColumns.Append(", ");
            }

            selectColumns.Append(column.SourceName);

            if (column.IsOrderedBy)
            {
                if (orderByColumns.Length > 0)
                {
                    orderByColumns.Append(", ");
                }

                orderByColumns.Append(column.SourceName);
            }
        }

        if (orderByColumns.Length > 0)
        {
            return $"SELECT {selectColumns} FROM {tableName} ORDER BY {orderByColumns}";
        }

        return $"SELECT {selectColumns} FROM {tableName}";
    }
}
