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
    public TimeRange(TimeSpan start, TimeSpan end) : this(start, end, () => end.Subtract(start))
    {
    }

    internal TimeRange(TimeSpan start, TimeSpan end, Func<TimeSpan> duration) : base(start, end, duration)
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
