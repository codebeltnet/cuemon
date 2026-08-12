using Cuemon.Configuration;

namespace Cuemon.Collections.Generic;
/// <summary>
/// Configuration options for <see cref="PaginationEnumerable{T}"/> and <see cref="PaginationList{T}"/>.
/// </summary>
public class PaginationOptions : IParameterObject
{
    private int _pageNumber;
    private int _pageSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="PaginationOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="PageNumber"/></term>
    ///         <description>1</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="PageSize"/></term>
    ///         <description>25</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public PaginationOptions()
    {
        PageSize = 25;
        PageNumber = 1;
    }

    /// <summary>
    /// Gets or sets the number of elements to display on a page.
    /// </summary>
    /// <value>The number of elements to display on a page.</value>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 0 ? 0 : value;
    }

    /// <summary>
    /// Gets or sets the one-based number of the page to iterate.
    /// </summary>
    /// <value>The one-based number of the page to iterate.</value>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }
}
