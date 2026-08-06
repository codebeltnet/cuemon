using System.Threading;
using Cuemon.Threading;

namespace Cuemon.Resilience;
/// <summary>
/// Specifies options that is related to the <see cref="TransientOperation"/> class.
/// </summary>
public sealed class AsyncTransientOperationOptions : TransientOperationOptions, IAsyncOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TransientOperationOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="TransientOperationOptions"/>.
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
    public AsyncTransientOperationOptions()
    {
        CancellationToken = default;
    }

    /// <summary>
    /// Gets or sets the cancellation token of an asynchronous operations.
    /// </summary>
    /// <value>The cancellation token of an asynchronous operations.</value>
    public CancellationToken CancellationToken { get; set; }
}
