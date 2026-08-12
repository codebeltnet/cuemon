using System;
using Cuemon.Threading;

namespace Cuemon.IO;
/// <summary>
/// Configuration options for <see cref="IDisposable"/>.
/// </summary>
public class AsyncDisposableOptions : AsyncOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncDisposableOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AsyncDisposableOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="LeaveOpen"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AsyncDisposableOptions()
    {
        LeaveOpen = false;
    }

    /// <summary>
    /// Gets or sets a value indicating whether a disposable object should bypass the mechanism for releasing unmanaged resources. Default is <c>false</c>.
    /// </summary>
    /// <value><c>true</c> if a disposable object should bypass the mechanism for releasing unmanaged resources; otherwise, <c>false</c>.</value>
    public bool LeaveOpen { get; set; }
}
