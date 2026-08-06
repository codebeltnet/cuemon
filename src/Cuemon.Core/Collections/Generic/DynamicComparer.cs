using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic;
/// <summary>
/// Provides a factory based way to create and wrap an <see cref="IComparer{T}"/> implementation.
/// </summary>
public static class DynamicComparer
{
    /// <summary>
    /// Creates a dynamic instance of an <see cref="IComparer{T}"/> implementation wrapping <see cref="IComparer{T}.Compare"/> through <paramref name="comparer"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="comparer">The function delegate that performs a comparison of two objects of the same type and returns a value indicating whether one object is less than, equal to, or greater than the other.</param>
    /// <returns>A dynamic instance of <see cref="IComparer{T}"/> that serves as a sort order comparer for type <typeparamref name="T"/>.</returns>
    public static IComparer<T> Create<T>(Func<T, T, int> comparer)
    {
        return new DynamicComparer<T>(comparer);
    }
}

internal sealed class DynamicComparer<T> : Comparer<T>
{
    internal DynamicComparer(Func<T, T, int> comparer)
    {
        Validator.ThrowIfNull(comparer);

        Comparer = comparer;
    }

    private Func<T, T, int> Comparer { get; set; }

    public override int Compare(T x, T y)
    {
        return Comparer(x, y);
    }
}
