using System.Threading;
using Cuemon.Threading;

namespace Cuemon.Diagnostics;
/// <summary>
/// Specifies options that is related to <see cref="TimeMeasure" /> operations. This class cannot be inherited.
/// </summary>
/// <seealso cref="TimeMeasureOptions" />
public sealed class AsyncTimeMeasureOptions : TimeMeasureOptions, IAsyncOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeMeasureOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="TimeMeasureOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="CancellationToken"/></term>
    ///         <description><c>default</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AsyncTimeMeasureOptions()
    {
        CancellationToken = default;
    }

    /// <summary>
    /// Gets or sets the cancellation token of an asynchronous operations.
    /// </summary>
    /// <value>The cancellation token of an asynchronous operations.</value>
    public CancellationToken CancellationToken { get; set; }
}
