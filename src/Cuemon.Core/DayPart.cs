using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cuemon;
/// <summary>
/// Represents a part of a day.
/// The built-in parts of day supports a roughly way to determine whether it is; <see cref="Night"/>, <see cref="Morning"/>, <see cref="Forenoon"/>, <see cref="Afternoon"/> or <see cref="Evening"/>.
/// Keep in mind that there is no exact science for day parts; it is as much a cultural as it is a personal preference.
/// </summary>
public class DayPart
{
    /// <summary>
    /// Gets the day part of a 24-hour period that approximates to Night.
    /// </summary>
    /// <value>The range of a 24-hour period that approximates to Night.</value>
    public static DayPart Night => new("Night", new TimeRange(TimeSpan.FromHours(21), TimeSpan.FromHours(3), () => TimeSpan.FromHours(6)));

    /// <summary>
    /// Gets the day part of a 24-hour period that approximates to Morning.
    /// </summary>
    /// <value>The range of a 24-hour period that approximates to Morning.</value>
    public static DayPart Morning => new("Morning", new TimeRange(Night.Range.End, Night.Range.End.Add(TimeSpan.FromHours(6))));

    /// <summary>
    /// Gets the day part of a 24-hour period that approximates to Forenoon.
    /// </summary>
    /// <value>The range of a 24-hour period that approximates to Forenoon.</value>
    public static DayPart Forenoon => new("Forenoon", new TimeRange(Morning.Range.End, Morning.Range.End.Add(TimeSpan.FromHours(3))));

    /// <summary>
    /// Gets the day part of a 24-hour period that approximates to Afternoon.
    /// </summary>
    /// <value>The range of a 24-hour period that approximates to Afternoon.</value>
    public static DayPart Afternoon => new("Afternoon", new TimeRange(Forenoon.Range.End, Forenoon.Range.End.Add(TimeSpan.FromHours(6))));

    /// <summary>
    /// Gets the day part of a 24-hour period that approximates to Evening.
    /// </summary>
    /// <value>The range of a 24-hour period that approximates to Evening.</value>
    public static DayPart Evening => new("Evening", new TimeRange(Afternoon.Range.End, Afternoon.Range.End.Add(TimeSpan.FromHours(3))));

    /// <summary>
    /// Gets the day parts of a 24-hour range of period.
    /// </summary>
    /// <value>The day parts of a 24-hour range of period.</value>
    public static IEnumerable<DayPart> All
    {
        get
        {
            yield return Night;
            yield return Morning;
            yield return Forenoon;
            yield return Afternoon;
            yield return Evening;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DayPart"/> class.
    /// </summary>
    /// <param name="name">The name of the part of a day.</param>
    /// <param name="range">The time range to cover.</param>
    public DayPart(string name, TimeRange range)
    {
        Validator.ThrowIfNullOrWhitespace(name);
        Validator.ThrowIfGreaterThan(range.Duration.TotalHours, 24, nameof(range), "A day part cannot exceed a period of 24 hours.");

        Name = name;
        Range = range;
    }

    /// <summary>
    /// Gets the name of a <see cref="DayPart"/>.
    /// </summary>
    /// <value>The name of a <see cref="DayPart"/>.</value>
    public string Name { get; }

    /// <summary>
    /// Gets the approximate range that this <see cref="DayPart"/> represents.
    /// </summary>
    /// <value>The approximate range that this <see cref="DayPart"/> represents.</value>
    public TimeRange Range { get; set; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return FormattableString.Invariant($"{Name} ({Range.Start.ToString("t", CultureInfo.InvariantCulture)} - {Range.End.ToString("t", CultureInfo.InvariantCulture)})");
    }
}
