using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Collections.Generic;
/// <summary>
/// Exposes the enumerator, which supports iteration in partitions over a collection of a specified type.
/// Implements the <see cref="IEnumerable{T}" />
/// </summary>
/// <typeparam name="T">The type of objects to enumerate.</typeparam>
/// <seealso cref="IEnumerable{T}" />
public class PartitionerEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _source;
    private readonly int _partitionSize;
    private int _iteratedCount;
    private bool _endOfSequence;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionerEnumerable{T}"/> class.
    /// </summary>
    /// <param name="source">The sequence to iterate in partitions.</param>
    /// <param name="partitionSize">The size of the partitions.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="partitionSize" /> is lower than 0.
    /// </exception>
    public PartitionerEnumerable(IEnumerable<T> source, int partitionSize = 128)
    {
        Validator.ThrowIfLowerThan(partitionSize, 0, nameof(partitionSize));
        _source = source ?? Enumerable.Empty<T>();
        _partitionSize = partitionSize;
    }

    /// <summary>
    /// Gets the sequence that this instance was constructed with.
    /// </summary>
    /// <value>The sequence that this instance was constructed with.</value>
    protected IEnumerable<T> Origin => _source;

    /// <summary>
    /// Gets the number of elements per partition.
    /// </summary>
    /// <value>The number of elements per partition.</value>
    public int PartitionSize => _partitionSize;

    /// <summary>
    /// Gets the number of times the this instance was iterated.
    /// </summary>
    /// <value>The number of times the this instance was iterated.</value>
    public int IteratedCount => _iteratedCount;

    /// <summary>
    /// Gets a value indicating whether this instance has partitions remaining to be iterated.
    /// </summary>
    /// <value><c>true</c> if this instance has partitions remaining to be iterated; otherwise, <c>false</c>.</value>
    public bool HasPartitions => !_endOfSequence;

    /// <summary>
    /// Returns an enumerator that iterates through the partition of the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the partition of the collection.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return new PartitionerEnumerator<T>(_source.Skip(IteratedCount).GetEnumerator(), PartitionSize, () => _iteratedCount++, () => _endOfSequence = true);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
