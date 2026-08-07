namespace Cuemon.Data;
/// <summary>
/// Identifies the format for a query fragment.
/// </summary>
public enum QueryFormat
{
    /// <summary>
    /// Indicates that the query fragment should be in the format; value, value, value.
    /// </summary>
    Delimited = 0,
    /// <summary>
    /// Indicates that the query fragment should be in the format; 'value', 'value', 'value'.
    /// </summary>
    DelimitedString = 1,
    /// <summary>
    /// Indicates that the query fragment should be in the format; [value], [value], [value].
    /// </summary>
    DelimitedSquareBracket = 2
}
