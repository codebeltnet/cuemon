namespace Cuemon;
/// <summary>
/// Defines the default pattern to use when formatting date- and time values.
/// </summary>
public enum DateTimeFormatPattern
{
    /// <summary>
    /// Displays a date using the short-date format.
    /// </summary>
    ShortDate,
    /// <summary>
    /// Displays a date using the long-date format.
    /// </summary>
    LongDate,
    /// <summary>
    /// Displays a time using the short-time format.
    /// </summary>
    ShortTime,
    /// <summary>
    /// Displays a time using the long-time format.
    /// </summary>
    LongTime,
    /// <summary>
    /// Displays a date using the short-date format in conjunction with the short-time format.
    /// </summary>
    ShortDateTime,
    /// <summary>
    /// Displays a date using the long-date format in conjunction with the long-time format.
    /// </summary>
    LongDateTime
}
