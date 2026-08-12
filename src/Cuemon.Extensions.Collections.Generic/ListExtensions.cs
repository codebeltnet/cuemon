using System;
using System.Collections.Generic;

namespace Cuemon.Extensions.Collections.Generic;
/// <summary>
/// Extension methods for the <see cref="IList{T}"/> interface.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Removes the first occurrence of a specific object from the <paramref name="list"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="IList{T}"/>.</typeparam>
    /// <param name="list">The <see cref="IList{T}"/> to extend.</param>
    /// <param name="predicate">The function delegate that defines the conditions of the element to remove.</param>
    /// <returns><c>true</c> if item was successfully removed from the <paramref name="list"/>, <c>false</c> otherwise.</returns>
    public static bool Remove<T>(this IList<T> list, Func<T, bool> predicate)
    {
        for (var i = 0; i < list.Count; i++)
        {
            if (predicate(list[i]))
            {
                list.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the <paramref name="list"/> of the <see cref="IList{T}"/> is within the range of the <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="IList{T}"/>.</typeparam>
    /// <param name="list">The <see cref="IList{T}"/> to extend.</param>
    /// <param name="index">The index to find.</param>
    /// <returns><c>true</c> if the specified <paramref name="index"/> is within the range of the <paramref name="list"/>; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="list"/> is null.
    /// </exception>
    public static bool HasIndex<T>(this IList<T> list, int index)
    {
        Validator.ThrowIfNull(list);
        return ((list.Count - 1) >= index);
    }

    /// <summary>
    /// Returns the next element of <paramref name="list"/> relative to <paramref name="index"/>, or the last element of <paramref name="list"/> if <paramref name="index"/> is equal or greater than <see cref="ICollection{T}.Count"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="IList{T}"/>.</typeparam>
    /// <param name="list">The <see cref="IList{T}"/> to extend.</param>
    /// <param name="index">The index of which to advance to the next element from.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="list"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is less than 0.
    /// </exception>
    /// <returns>default(TSource) if <paramref name="index"/> is equal or greater than <see cref="ICollection{T}.Count"/>; otherwise the next element of <paramref name="list"/> relative to <paramref name="index"/>.</returns>
    public static T Next<T>(this IList<T> list, int index)
    {
        Validator.ThrowIfNull(list);
        Validator.ThrowIfLowerThan(index, 0, nameof(index));
        var nextIndex = index + 1;
        if (nextIndex >= list.Count) { return default; }
        return list[nextIndex];
    }

    /// <summary>
    /// Returns the previous element of <paramref name="list"/> relative to <paramref name="index"/>, or the first or last element of <paramref name="list"/> if <paramref name="index"/> is equal, greater or lower than <see cref="ICollection{T}.Count"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="IList{T}"/>.</typeparam>
    /// <param name="list">The <see cref="IList{T}"/> to extend.</param>
    /// <param name="index">The index of which to advance to the previous element from.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="list"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is less than 0.
    /// </exception>
    /// <returns>default(TSource) if <paramref name="index"/> is equal, greater or lower than <see cref="ICollection{T}.Count"/>; otherwise the previous element of <paramref name="list"/> relative to <paramref name="index"/>.</returns>
    public static T Previous<T>(this IList<T> list, int index)
    {
        Validator.ThrowIfNull(list);
        Validator.ThrowIfLowerThan(index, 0, nameof(index));
        var previousIndex = index - 1;
        if (previousIndex < 0) { return default; }
        if (previousIndex >= list.Count) { return default; }
        return list[previousIndex];
    }

    /// <summary>
    /// Attempts to add the specified <paramref name="item"/> to the <paramref name="list"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The <see cref="IList{T}"/> to extend.</param>
    /// <param name="item">The item to add.</param>
    /// <returns><c>true</c> if the item was added to the <paramref name="list"/> successfully, <c>false</c> otherwise.</returns>
    /// <remarks>This method will add the specified <paramref name="item"/> to the list if it is not already present.</remarks>
    public static bool TryAdd<T>(this IList<T> list, T item)
    {
        Validator.ThrowIfNull(list);
        if (!list.Contains(item))
        {
            list.Add(item);
            return true;
        }
        return false;
    }
}
