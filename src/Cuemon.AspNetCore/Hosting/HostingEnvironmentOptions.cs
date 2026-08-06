using System;
using Cuemon.Configuration;
using Microsoft.Extensions.Hosting;

namespace Cuemon.AspNetCore.Hosting;
/// <summary>
/// Configuration options for <see cref="HostingEnvironmentMiddleware"/>.
/// </summary>
public class HostingEnvironmentOptions : IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HostingEnvironmentOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="HostingEnvironmentOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="HeaderName"/></term>
    ///         <description><c>X-Hosting-Environment</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="SuppressHeaderPredicate"/></term>
    ///         <description><c>environment => environment.IsProduction()</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public HostingEnvironmentOptions()
    {
        HeaderName = "X-Hosting-Environment";
        SuppressHeaderPredicate = environment => environment.IsProduction();
    }

    /// <summary>
    /// Gets or sets the name of the hosting environment HTTP header.
    /// </summary>
    /// <value>The name of the hosting environment HTTP header.</value>
    public string HeaderName { get; set; }

    /// <summary>
    /// Gets or sets the predicate that can suppress the hosting environment HTTP header.
    /// </summary>
    /// <value>The function delegate that can determine if the hosting environment HTTP header should be suppressed.</value>
    public Func<IHostEnvironment, bool> SuppressHeaderPredicate { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="HeaderName"/> cannot be null, empty or consist only of white-space characters - or -
    /// <see cref="SuppressHeaderPredicate"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName));
        Validator.ThrowIfInvalidState(SuppressHeaderPredicate == null);
    }
}
