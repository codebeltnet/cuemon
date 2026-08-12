using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.Extensions.Collections.Generic;
/// <summary>
/// Extension methods for the <see cref="IEnumerable{T}"/> interface.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Returns a chunked <see cref="IEnumerable{T}"/> sequence with a maximum of the specified <paramref name="size"/>. Default is 128.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <param name="size">The amount of elements to process at a time.</param>
    /// <returns>An <see cref="IEnumerable{T}" /> that contains no more than the specified <paramref name="size" /> of elements from the <paramref name="source" /> sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="size"/> is less or equal to 0.
    /// </exception>
    /// <remarks>The original <paramref name="source"/> is reduced equivalent to the number of elements in the returned sequence.</remarks>
    public static PartitionerEnumerable<T> Chunk<T>(this IEnumerable<T> source, int size = 128)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfLowerThanOrEqual(size, 0, nameof(size));
        return new PartitionerEnumerable<T>(source, size);
    }

    /// <summary>
    /// Shuffles the specified <paramref name="source"/> like a deck of cards.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <returns>A sequence of <typeparamref name="T"/> with the shuffled <paramref name="source"/>.</returns>
    /// <remarks>Fisher–Yates shuffle: https://en.wikipedia.org/wiki/Fisher–Yates_shuffle</remarks>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.Shuffle(Generate.RandomNumber);
    }

    /// <summary>
    /// Shuffles the specified <paramref name="source"/> like a deck of cards.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <param name="randomizer">The function delegate that will handle the randomization of <paramref name="source"/>.</param>
    /// <returns>A sequence of <typeparamref name="T"/> with the shuffled <paramref name="source"/>.</returns>
    /// <remarks>Fisher–Yates shuffle: https://en.wikipedia.org/wiki/Fisher–Yates_shuffle</remarks>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Func<int, int, int> randomizer)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(randomizer);
        var buffer = source.ToArray();
        var length = buffer.Length;
        while (length > 0)
        {
            length--;
            var random = randomizer(0, length + 1);
            var shuffled = buffer[random];
            yield return shuffled;
            buffer[random] = buffer[length];
        }
    }

    /// <summary>
    /// Returns ascending sorted elements from a sequence by using the default comparer to compare values.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains ascending sorted elements from the source sequence.</returns>
    public static IEnumerable<T> OrderAscending<T>(this IEnumerable<T> source)
    {
        return source.OrderAscending(Comparer<T>.Default);
    }

    /// <summary>
    /// Returns ascending sorted elements from a sequence by using a specified <see cref="IComparer{T}"/> to compare values.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare values.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains ascending sorted elements from the source sequence.</returns>
    public static IEnumerable<T> OrderAscending<T>(this IEnumerable<T> source, IComparer<T> comparer)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(comparer);
        return source.OrderBy(t => t, comparer);
    }

    /// <summary>
    /// Returns descending sorted elements from a sequence by using the default comparer to compare values.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains descending sorted elements from the source sequence.</returns>
    public static IEnumerable<T> OrderDescending<T>(this IEnumerable<T> source)
    {
        return source.OrderDescending(Comparer<T>.Default);
    }

    /// <summary>
    /// Returns descending sorted elements from a sequence by using a specified <see cref="IComparer{T}"/> to compare values.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare values.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains descending sorted elements from the source sequence.</returns>
    public static IEnumerable<T> OrderDescending<T>(this IEnumerable<T> source, IComparer<T> comparer)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(comparer);
        return source.OrderByDescending(t => t, comparer);
    }

    /// <summary>
    /// Returns a random element of a sequence of elements, or a default value if no element is found.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <returns><c>default</c> if <paramref name="source"/> is empty; otherwise, a random element of <paramref name="source"/>.</returns>
    public static T RandomOrDefault<T>(this IEnumerable<T> source)
    {
        Validator.ThrowIfNull(source);
        var collection = source as ICollection<T> ?? new List<T>(source);
        return collection.Count == 0 ? default : collection.ElementAt(Generate.RandomNumber(collection.Count));
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="value"/> as the only element.
    /// </summary>
    /// <typeparam name="T">The type of the element of <see cref="IEnumerable{T}"/>.</typeparam>
    /// <param name="value">The <typeparamref name="T"/> to extend.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="value"/> as the only element.</returns>
    public static IEnumerable<T> Yield<T>(this T value)
    {
        return Arguments.Yield(value);
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey,TValue}"/> from the specified <paramref name="source"/> sequence.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the <paramref name="source"/>.</typeparam>
    /// <typeparam name="TValue">The type of values in the <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <returns>A <see cref="Dictionary{TKey,TValue}"/> that is equivalent to the specified <paramref name="source"/> sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="source"/> contains at least one <see cref="KeyValuePair{TKey,TValue}"/> that produces duplicate keys for two elements.
    /// </exception>
    public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
    {
        return ToDictionary(source, EqualityComparer<TKey>.Default);
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey,TValue}"/> from the specified <paramref name="source"/> sequence.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the <paramref name="source"/>.</typeparam>
    /// <typeparam name="TValue">The type of values in the <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing keys.</param>
    /// <returns>A <see cref="Dictionary{TKey,TValue}"/> that is equivalent to the specified <paramref name="source"/> sequence.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is null - or - <paramref name="comparer"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="source"/> contains at least one <see cref="KeyValuePair{TKey,TValue}"/> that produces duplicate keys for two elements.
    /// </exception>
    public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey> comparer)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(comparer);

        var result = new Dictionary<TKey, TValue>(comparer);
        foreach (var item in source)
        {
            result.Add(item.Key, item.Value);
        }
        return result;
    }

    /// <summary>
    /// Extends the specified <paramref name="source"/> to support iterating in partitions.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <param name="partitionSize">The size of the partitions.</param>
    /// <returns>An instance of <see cref="PartitionerEnumerable{T}"/>.</returns>
    public static PartitionerEnumerable<T> ToPartitioner<T>(this IEnumerable<T> source, int partitionSize = 128)
    {
        return new PartitionerEnumerable<T>(source, partitionSize);
    }

    /// <summary>
    /// Converts the specified <paramref name="source"/> to a generic and read-only pagination sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <param name="totalElementCounter">The total element counter.</param>
    /// <param name="setup">The <see cref="PaginationOptions"/> which may be configured.</param>
    /// <returns>An instance of <see cref="PaginationEnumerable{T}"/>.</returns>
    public static PaginationEnumerable<T> ToPagination<T>(this IEnumerable<T> source, Func<int> totalElementCounter, Action<PaginationOptions> setup = null)
    {
        return new PaginationEnumerable<T>(source, totalElementCounter, setup);
    }

    /// <summary>
    /// Converts the specified <paramref name="source"/> to an eagerly materialized generic and read-only pagination list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend.</param>
    /// <param name="totalElementCounter">The total element counter.</param>
    /// <param name="setup">The <see cref="PaginationOptions"/> which may be configured.</param>
    /// <returns>An instance of <see cref="PaginationList{T}"/>.</returns>
    public static PaginationList<T> ToPaginationList<T>(this IEnumerable<T> source, Func<int> totalElementCounter, Action<PaginationOptions> setup = null)
    {
        return new PaginationList<T>(source, totalElementCounter, setup);
    }
}
