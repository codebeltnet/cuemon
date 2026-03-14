using System;
using System.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Represents a period of time between two <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the time measurement.</typeparam>
    /// <seealso cref="Range{T}" />
    public abstract class Range<T> : IEqualityComparer<Range<T>> where T : IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Range{T}"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="durationResolver">The duration resolver.</param>
        protected Range(T start, T end, Func<TimeSpan> durationResolver)
        {
            Start = start;
            End = end;
            Duration = durationResolver();
        }

        /// <summary>
        /// Gets the point of time where this time range begin.
        /// </summary>
        /// <value>A value representing the point of time where this time range begin.</value>
        public T Start { get; }

        /// <summary>
        /// Gets the point of time where this time range end.
        /// </summary>
        /// <value>A value representing the point of time where this time range end.</value>
        public T End { get; }

        /// <summary>
        /// Gets the duration between <see cref="Start"/> and <see cref="End"/>.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> representing the duration between <see cref="Start"/> and <see cref="End"/>.</value>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="format">A composite format string for the <typeparamref name="T"/> properties.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public virtual string ToString(string format, IFormatProvider provider)
        {
            FormattableString fs = $"A duration of {Duration.Days:D2}.{Duration.Hours:D2}:{Duration.Minutes:D2}:{Duration.Seconds:D2} between {Start.ToString(format, provider)} and {End.ToString(format, provider)}.";
            return fs.ToString(provider);
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <typeparamref name="T"/> to compare.</param>
        /// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
        public bool Equals(Range<T> x, Range<T> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null) return false;
            if (y is null) return false;
            if (x.GetType() != y.GetType()) return false;
            return EqualityComparer<T>.Default.Equals(x.Start, y.Start) && EqualityComparer<T>.Default.Equals(x.End, y.End);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(Range<T> obj)
        {
            return obj.Start.GetHashCode() ^ obj.End.GetHashCode();
        }
    }
}