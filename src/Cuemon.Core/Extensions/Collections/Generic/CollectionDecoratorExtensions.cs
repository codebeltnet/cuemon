using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic;
/// <summary>
/// Extension methods for the <see cref="ICollection{T}"/> interface hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class CollectionDecoratorExtensions
{
    /// <summary>
    /// Adds the elements of the specified <paramref name="source"/> to the enclosed <see cref="ICollection{T}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <param name="decorator">The decorator that wraps the <see cref="ICollection{T}"/> to extend.</param>
    /// <param name="source">The sequence of elements that should be added to the enclosed <see cref="ICollection{T}"/> of the <paramref name="decorator"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static void AddRange<T>(this IDecorator<ICollection<T>> decorator, params T[] source)
    {
        AddRange(decorator, (IEnumerable<T>)source);
    }

    /// <summary>
    /// Adds the elements of the specified <paramref name="source"/> to the enclosed <see cref="ICollection{T}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <param name="decorator">The decorator that wraps the <see cref="ICollection{T}"/> to extend.</param>
    /// <param name="source">The sequence of elements that should be added to the enclosed <see cref="ICollection{T}"/> of the <paramref name="decorator"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static void AddRange<T>(this IDecorator<ICollection<T>> decorator, IEnumerable<T> source)
    {
        Validator.ThrowIfNull(decorator);
        if (decorator.Inner is List<T> list)
        {
            list.AddRange(source);
            return;
        }
        foreach (var item in source) { decorator.Inner.Add(item); }
    }
}
