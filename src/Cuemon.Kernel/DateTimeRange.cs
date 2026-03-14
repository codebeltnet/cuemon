using System;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// Represents a period of time between two <see cref="DateTime"/> values.
    /// </summary>
    public class DateTimeRange : Range<DateTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> struct.
        /// </summary>
        /// <param name="start">The start date of a time range.</param>
        /// <param name="end">The end date of a time range.</param>
        public DateTimeRange(DateTime start, DateTime end) : base(start, end, () => end.Subtract(start))
        {
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("s", CultureInfo.InvariantCulture);
        }
    }
}
