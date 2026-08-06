namespace Cuemon.Data;
/// <summary>
/// Identifies the type of data operation performed by a query against a data source.
/// </summary>
public enum QueryType
{
    /// <summary>
    /// Indicates that a query is used for a data operation that retrieves data.
    /// </summary>
    Select = 0,
    /// <summary>
    /// Indicates that a query is used for a data operation that updates data.
    /// </summary>
    Update = 1,
    /// <summary>
    /// Indicates that a query is used for a data operation that inserts data.
    /// </summary>
    Insert = 2,
    /// <summary>
    /// Indicates that a query is used for a data operation that deletes data.
    /// </summary>
    Delete = 3,
    /// <summary>
    /// Indicates that a query is specifically used for a lookup on whether a data record exists.
    /// </summary>
    Exists = 4
}
