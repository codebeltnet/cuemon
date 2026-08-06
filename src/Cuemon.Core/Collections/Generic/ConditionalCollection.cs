using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Collections.Generic;
/// <summary>
/// Provides the abstract base class for a generic, conditional collection.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
/// <seealso cref="ICollection{T}" />
public abstract class ConditionalCollection<T> : ICollection<T>
{
    private readonly List<T> _wrapper = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionalCollection{T}"/> class.
    /// </summary>
    protected ConditionalCollection()
    {
    }

    /// <summary>
    /// Gets the number of elements contained in the <see cref="ConditionalCollection{T}" />.
    /// </summary>
    /// <value>The number of elements actually contained in the <see cref="ConditionalCollection{T}"/>.</value>
    public int Count => _wrapper.Count;

    /// <summary>
    /// Gets a value indicating whether the <see cref="ConditionalCollection{T}" /> is read-only.
    /// </summary>
    /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
    public bool IsReadOnly => false;

    /// <summary>
    /// Adds an item to the <see cref="ConditionalCollection{T}" />.
    /// </summary>
    /// <param name="item">The object to add to the <see cref="ConditionalCollection{T}" />.</param>
    public abstract void Add(T item);

    /// <summary>
    /// Adds an item to the <see cref="ConditionalCollection{T}" />.
    /// </summary>
    /// <param name="item">The object to add to the <see cref="ConditionalCollection{T}" />.</param>
    /// <param name="validator">The delegate that validates the <paramref name="item"/> being added to the <see cref="ConditionalCollection{T}" />.</param>
    protected void Add(T item, Action validator)
    {
        validator();
        _wrapper.Add(item);
    }

    /// <summary>
    /// Removes the first occurrence of a specific object from the <see cref="ConditionalCollection{T}" />.
    /// </summary>
    /// <param name="item">The object to remove from the <see cref="ConditionalCollection{T}" />.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was successfully removed from the <see cref="ConditionalCollection{T}" />; otherwise, <c>false</c>. This method also returns false if <paramref name="item" /> is not found in the original <see cref="ConditionalCollection{T}" />.</returns>
    public abstract bool Remove(T item);

    /// <summary>
    /// Removes all occurrences of a specific object from the <see cref="ConditionalCollection{T}" /> that match the conditions defined by the specified <paramref name="predicate"/> and <paramref name="comparer"/>.
    /// </summary>
    /// <param name="item">The object to remove from the <see cref="ConditionalCollection{T}" />.</param>
    /// <param name="predicate">The function delegate that will iterate and match the specified <paramref name="item"/> from the <see cref="ConditionalCollection{T}"/>.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing the specified <paramref name="item"/> with an element from the <see cref="ConditionalCollection{T}"/>.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was successfully removed from the <see cref="ConditionalCollection{T}" />; otherwise, <c>false</c>. This method also returns false if <paramref name="item" /> is not found in the original <see cref="ConditionalCollection{T}" />.</returns>
    protected bool Remove(T item, Func<T, bool> predicate, IEqualityComparer<T> comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;
        predicate ??= x => comparer.Equals(item, x) && (comparer.GetHashCode(item) == comparer.GetHashCode(x));
        var match = new Predicate<T>(predicate);
        return _wrapper.RemoveAll(match) > 0;
    }

    /// <summary>
    /// Determines whether the <see cref="ConditionalCollection{T}" /> contains a specific value.
    /// </summary>
    /// <param name="item">The object to locate in the <see cref="ConditionalCollection{T}" />.</param>
    /// <returns><c>true</c> if <paramref name="item" /> is found in the <see cref="ConditionalCollection{T}" />; otherwise, <c>false</c>.</returns>
    public abstract bool Contains(T item);

    /// <summary>
    /// Determines whether the <see cref="ConditionalCollection{T}" /> contains a specific value by using a specified <paramref name="comparer"/>.
    /// </summary>
    /// <param name="item">The object to locate in the <see cref="ConditionalCollection{T}" />.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing the specified <paramref name="item"/> with an element from the <see cref="ConditionalCollection{T}"/>.</param>
    /// <returns><c>true</c> if <paramref name="item" /> is found in the <see cref="ConditionalCollection{T}" />; otherwise, <c>false</c>.</returns>
    protected bool Contains(T item, IEqualityComparer<T> comparer)
    {
        comparer ??= EqualityComparer<T>.Default;
        return _wrapper.Contains(item, comparer);
    }

    /// <summary>
    /// Removes all items from the <see cref="ConditionalCollection{T}" />.
    /// </summary>
    public void Clear()
    {
        _wrapper.Clear();
    }

    /// <summary>
    /// Copies the elements of the <see cref="ConditionalCollection{T}" /> to an <see cref="Array" />, starting at a particular <see cref="Array" /> index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="ConditionalCollection{T}" />. The <see cref="Array" /> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        _wrapper.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return _wrapper.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
