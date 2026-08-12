using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Collections.Generic;
/// <summary>
/// Represents an eagerly materialized generic and read-only pagination list.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
/// <seealso cref="IReadOnlyList{T}" />
public class PaginationList<T> : PaginationEnumerable<T>, IReadOnlyList<T>
{
    private readonly IList<T> _pageSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationList{T}"/> class.
    /// </summary>
    /// <param name="source">The sequence to turn into a page.</param>
    /// <param name="totalElementCounter">The total element counter.</param>
    /// <param name="setup">The <see cref="PaginationOptions"/> which may be configured.</param>
    public PaginationList(IEnumerable<T> source, Func<int> totalElementCounter, Action<PaginationOptions> setup = null) : base(source, totalElementCounter, setup)
    {
        _pageSource = PageSource.ToList();
    }

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    public T this[int index] => _pageSource[index];

    /// <summary>
    /// Gets the number of elements on the current page.
    /// </summary>
    /// <value>The number of elements on the current page.</value>
    public int Count => _pageSource.Count;
}
