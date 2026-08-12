using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Collections.Generic;
/// <summary>
/// Represents a generic and read-only pagination sequence.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
/// <seealso cref="IEnumerable{T}" />
public class PaginationEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _source;
    private readonly int _totalElementCount;
    private readonly int _pageSize;
    private readonly int _pageNumber;
    private int _pageCount = -1;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationEnumerable{T}"/> class.
    /// </summary>
    /// <param name="source">The sequence to turn into a page.</param>
    /// <param name="totalElementCounter">The total element counter.</param>
    /// <param name="setup">The <see cref="PaginationOptions"/> which may be configured.</param>
    public PaginationEnumerable(IEnumerable<T> source, Func<int> totalElementCounter, Action<PaginationOptions> setup = null)
    {
        Validator.ThrowIfNull(totalElementCounter);
        source ??= Enumerable.Empty<T>();
        var options = Patterns.Configure(setup);
        _source = options.PageNumber == 1
            ? source.Take(options.PageSize)
            : source.Skip((options.PageNumber - 1) * options.PageSize).Take(options.PageSize);
        _pageSize = options.PageSize;
        _pageNumber = options.PageNumber;
        _totalElementCount = totalElementCounter();
    }

    /// <summary>
    /// Gets the page source of this instance.
    /// </summary>
    /// <value>The page source of this instance.</value>
    protected IEnumerable<T> PageSource => _source;

    /// <summary>
    /// Gets the total amount of pages for the elements in this sequence.
    /// </summary>
    /// <value>The total amount of pages for the elements in this sequence.</value>
    public int PageCount
    {
        get
        {
            if (_pageCount < 0)
            {
                if (_totalElementCount == 0) { return 0; }
                var intermediateResult = _totalElementCount / Decorator.Enclose<object>(_pageSize).ChangeTypeOrDefault<double>();
                _pageCount = Decorator.Enclose<object>(Math.Ceiling(intermediateResult)).ChangeType<int>();
            }
            return _pageCount;
        }
    }

    /// <summary>
    /// Gets the total number of elements in the sequence before paging is applied.
    /// </summary>
    /// <value>The total number of elements in the sequence before paging is applied.</value>
    public int TotalElementCount => _totalElementCount;

    /// <summary>
    /// Gets a value indicating whether this instance has a next paged data sequence.
    /// </summary>
    /// <value><c>true</c> if this instance has a next paged data sequence; otherwise, <c>false</c>.</value>
    public bool HasNextPage => _pageNumber < PageCount;

    /// <summary>
    /// Gets a value indicating whether this instance has a previous paged data sequence.
    /// </summary>
    /// <value><c>true</c> if this instance has a previous paged data sequence; otherwise, <c>false</c>.</value>
    public bool HasPreviousPage => _pageNumber > 1 &&
                                   _pageNumber <= PageCount;

    /// <summary>
    /// Gets a value indicating whether this instance is on the first paged data sequence.
    /// </summary>
    /// <value><c>true</c> if this instance is on the first paged data sequence; otherwise, <c>false</c>.</value>
    public bool FirstPage => _pageNumber == 1;

    /// <summary>
    /// Gets a value indicating whether this instance is on the last paged data sequence.
    /// </summary>
    /// <value><c>true</c> if this instance is on the last paged data sequence; otherwise, <c>false</c>.</value>
    public bool LastPage => _pageNumber == PageCount;

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return _source.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
