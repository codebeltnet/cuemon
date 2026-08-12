using System;
using System.Reflection;

namespace Cuemon.Reflection;
/// <summary>
/// Configuration options for <see cref="ActivatorFactory"/>.
/// </summary>
public class ActivatorOptions : FormattingOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActivatorOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="ActivatorOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Flags"/></term>
    ///         <description><c>BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Binder"/></term>
    ///         <description><see cref="Type.DefaultBinder"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public ActivatorOptions()
    {
        Binder = Type.DefaultBinder;
        Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
    }

    /// <summary>
    /// Gets the binding constraint used for discovering a suitable constructor.
    /// </summary>
    /// <value>The binding constraint used for discovering a suitable constructor.</value>
    public BindingFlags Flags { get; set; }

    /// <summary>
    /// Gets or sets the binder that uses <see cref="Flags"/> and the specified arguments to seek and identify the type constructor.
    /// </summary>
    /// <value>The binder that uses <see cref="Flags"/> and the specified arguments to seek and identify the type constructor.</value>
    public Binder Binder { get; set; }
}
