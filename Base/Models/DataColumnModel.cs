namespace FoodAndNutrientData.Base.Models;

/// <summary>
/// This class represents a column in a database.
/// </summary>
public class DataColumnModel
{
    /// <summary>
    /// The column name in the source database.
    /// </summary>
    public string SourceName { get; set; } = default!;

    /// <summary>
    /// The column name in the destination database.
    /// </summary>
    public string DestinationName { get; set; } = default!;

    /// <summary>
    /// True if this column should be ignored when setting model values to be
    /// stored in the destination database.
    /// </summary>
    public bool IsIgnored { get; set; }

    /// <summary>
    /// True if this column should be included in the ORDER BY statment when
    /// retrieving data from the source database.
    /// </summary>
    public bool IsOrderedBy { get; set; }

    /// <summary>
    /// The versions for which this column is applicable.
    /// </summary>
    public HashSet<int> Versions { get; set; } = default!;
}
