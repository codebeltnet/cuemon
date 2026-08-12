using Cuemon.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
/// <summary>
/// Specifies options that is related to <see cref="FaultDescriptorFilter" /> operations.
/// </summary>
/// <seealso cref="FaultDescriptorOptions" />
public class MvcFaultDescriptorOptions : FaultDescriptorOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MvcFaultDescriptorOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="MvcFaultDescriptorOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="MarkExceptionHandled"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public MvcFaultDescriptorOptions()
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether to mark ASP.NET Core MVC <see cref="ExceptionContext.ExceptionHandled"/> to <c>true</c>.
    /// </summary>
    /// <value><c>true</c> if <see cref="ExceptionContext.ExceptionHandled"/> should be set; otherwise, <c>false</c>.</value>
    public bool MarkExceptionHandled { get; set; }
}
