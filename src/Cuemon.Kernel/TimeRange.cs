using System;
using System.Globalization;

namespace Cuemon;
/// <summary>
/// Represents a period of time between two <see cref="TimeSpan"/> values.
/// </summary>
public class TimeRange : Range<TimeSpan>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeRange"/> struct.
    /// </summary>
    /// <param name="start">The start of a time range.</param>
    /// <param name="end">The end of a time range.</param>
    /// <param name="duration">A function delegate that returns the duration of the time range. If not provided, the duration is calculated as the difference between <paramref name="end"/> and <paramref name="start"/>.</param>
    public TimeRange(TimeSpan start, TimeSpan end, Func<TimeSpan> duration = null) : base(start, end, duration ?? (() => end.Subtract(start)))
    {
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return ToString("c", CultureInfo.InvariantCulture);
    }
}
