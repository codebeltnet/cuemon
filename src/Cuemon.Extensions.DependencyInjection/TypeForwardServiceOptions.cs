using System;
using System.Collections.Generic;
using Cuemon.Configuration;

namespace Cuemon.Extensions.DependencyInjection;
/// <summary>
/// Configuration options for Microsoft Dependency Injection that support nested type forwarding.
/// </summary>
/// <seealso cref="ServiceOptions"/>
/// <seealso cref="IValidatableParameterObject"/>
public class TypeForwardServiceOptions : ServiceOptions, IValidatableParameterObject
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
    ///         <term><see cref="NestedTypePredicate"/></term>
    ///         <description><c>serviceType => serviceType.GetInterfaces();</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="NestedTypeSelector"/></term>
    ///         <description><c>_ => true;</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public TypeForwardServiceOptions()
    {
        NestedTypeSelector = serviceType => serviceType.GetInterfaces();
        NestedTypePredicate = _ => true;
        UseNestedTypeForwarding = true;
    }

    /// <summary>
    /// Gets or sets a value indicating whether nested type forwarding should be part of the operation.
    /// </summary>
    /// <value><c>true</c> if nested type forwarding should be part of the operation; otherwise, <c>false</c>.</value>
    public bool UseNestedTypeForwarding { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that will test each element for a condition based on a <see cref="Type"/>.
    /// </summary>
    /// <value>The function delegate that will test each element for a condition based on a <see cref="Type"/>.</value>
    public Func<Type, bool> NestedTypePredicate { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that will fetch nested types of a service.
    /// </summary>
    /// <value>The function delegate that will fetch nested types of a service.</value>
    public Func<Type, IEnumerable<Type>> NestedTypeSelector { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="NestedTypePredicate"/> cannot be null - or -
    /// <see cref="NestedTypeSelector"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(NestedTypePredicate == null);
        Validator.ThrowIfInvalidState(NestedTypeSelector == null);
    }
}
