using System;

namespace Cuemon
{
    /// <summary>
    /// Specifies the unit of time - typically used with a <see cref="TimeSpan" />.
    /// </summary>
    public enum TimeUnit
    {
        /// <summary>
        /// Indicates a time unit of Days.
        /// </summary>
        Days,
        /// <summary>
        /// Indicates a time unit of Hours.
        /// </summary>
        Hours,
        /// <summary>
        /// Indicates a time unit of Minutes.
        /// </summary>
        Minutes,
        /// <summary>
        /// Indicates a time unit of Seconds.
        /// </summary>
        Seconds,
        /// <summary>
        /// Indicates a time unit of Milliseconds.
        /// </summary>
        Milliseconds,
        /// <summary>
        /// Indicates a time unit of Ticks, where one Tick is equal to 100 nanoseconds.
        /// </summary>
        Ticks
    }
}