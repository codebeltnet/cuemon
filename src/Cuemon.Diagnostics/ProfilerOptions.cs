using System;
using Cuemon.Configuration;
using Cuemon.Reflection;

namespace Cuemon.Diagnostics;
/// <summary>
/// Specifies options that is related to <see cref="Profiler"/> operations.
/// </summary>
public class ProfilerOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfilerOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="ProfilerOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="MethodDescriptor"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="RuntimeParameters"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    protected ProfilerOptions()
    {
    }

    /// <summary>
    /// Gets or sets the callback function delegate that resolves a <see cref="Reflection.MethodDescriptor"/>.
    /// </summary>
    /// <value>The callback function delegate that resolves a <see cref="Reflection.MethodDescriptor"/>.</value>
    public Func<MethodDescriptor> MethodDescriptor { get; set; }

    /// <summary>
    /// Gets or sets an <see cref="object"/> array that represents runtime values.
    /// </summary>
    /// <value>An <see cref="object"/> array that represents runtime values.</value>
    public object[] RuntimeParameters { get; set; }
}
