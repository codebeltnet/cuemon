using Cuemon.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.DependencyInjection;
/// <summary>
/// Configuration options for Microsoft Dependency Injection.
/// </summary>
/// <seealso cref="IParameterObject"/>
public class ServiceOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="ServiceOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Lifetime"/></term>
    ///         <description><see cref="ServiceLifetime.Transient"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public ServiceOptions()
    {
        Lifetime = ServiceLifetime.Transient;
    }

    /// <summary>
    /// Gets or sets the lifetime of the service to add.
    /// </summary>
    /// <value>The lifetime of the service.</value>
    public ServiceLifetime Lifetime { get; set; }
}
