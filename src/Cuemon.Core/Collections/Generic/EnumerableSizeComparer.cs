using System.Collections;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic;
/// <summary>
/// Provides <see cref="IEnumerable{T}"/> size comparison.
/// </summary>
/// <typeparam name="T">The <see cref="IEnumerable{T}"/> type to compare.</typeparam>
public class EnumerableSizeComparer<T> : Comparer<T> where T : IEnumerable
{
    /// <summary>
    /// Returns a default comparer for the type specified by the generic argument.
    /// </summary>
    public static new IComparer<T> Default => new EnumerableSizeComparer<T>();

    /// <summary>
    /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
    /// </summary>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>
    /// A signed integer that indicates the relative values of x and y, as explained here: Less than zero - x is less than y. Zero - x equals y. Greater than zero - x is greater than y.
    /// </returns>
    public override int Compare(T x, T y)
    {
        if (EqualityComparer<T>.Default.Equals(x, y)) { return 0; } // equivalent to both x and y are null
        if (EqualityComparer<T>.Default.Equals(x, default)) { return -1; } // equivalent to x == null
        if (EqualityComparer<T>.Default.Equals(default, y)) { return 1; } // equivalent to y == null

        var depthOfX = Count(x);
        var depthOfY = Count(y);

        if (depthOfX > depthOfY) { return 1; }
        if (depthOfX < depthOfY) { return -1; }

        return 0;
    }

    private static int Count(IEnumerable sequence) // compatible with netstandard2.0 --> ne9.0
    {
        var i = 0;
        foreach (var _ in sequence) { i++; }
        return i;
    }
}
