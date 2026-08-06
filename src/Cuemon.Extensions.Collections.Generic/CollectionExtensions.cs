using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon.Extensions.Collections.Generic;
/// <summary>
/// Extension methods for the <see cref="ICollection{T}"/> interface.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Extends the specified <paramref name="collection"/> to support iterating in partitions.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <paramref name="collection"/>.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}"/> to extend.</param>
    /// <param name="partitionSize">The size of the partitions.</param>
    /// <returns>An instance of <see cref="PartitionerCollection{T}"/>.</returns>
    public static PartitionerCollection<T> ToPartitioner<T>(this ICollection<T> collection, int partitionSize = 128)
    {
        Validator.ThrowIfNull(collection);
        return new PartitionerCollection<T>(collection, partitionSize);
    }

    /// <summary>
    /// Adds the elements of the specified <paramref name="source"/> to the <paramref name="collection"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}"/> to extend.</param>
    /// <param name="source">The sequence of elements that should be added to <paramref name="collection"/>.</param>
    public static void AddRange<T>(this ICollection<T> collection, params T[] source)
    {
        AddRange(collection, (IEnumerable<T>)source);
    }

    /// <summary>
    /// Adds the elements of the specified <paramref name="source"/> to the <paramref name="collection"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}"/> to extend.</param>
    /// <param name="source">The sequence of elements that should be added to <paramref name="collection"/>.</param>
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> source)
    {
        Validator.ThrowIfNull(collection);
        Validator.ThrowIfNull(source);
        if (collection is List<T> list)
        {
            list.AddRange(source);
            return;
        }
        foreach (var item in source) { collection.Add(item); }
    }
}
