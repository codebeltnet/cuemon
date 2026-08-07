using System;
using System.Reflection;
using Cuemon.Configuration;

namespace Cuemon.Reflection;
/// <summary>
/// Configuration options for <see cref="MethodBase"/>.
/// </summary>
/// <seealso cref="IParameterObject"/>
public class MethodBaseOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodBaseOptions"/> class.
    /// </summary>
    public MethodBaseOptions()
    {
        Comparison = StringComparison.Ordinal;
        Flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
    }

    /// <summary>
    /// Gets or sets the <see cref="BindingFlags"/> that specifies how the member search is conducted.
    /// </summary>
    /// <value>The <see cref="BindingFlags"/> that specifies how the member search is conducted.</value>
    public BindingFlags Flags { get; set; }

    /// <summary>
    /// Gets or sets the types representing the number, order, and type of the parameters for the member to resolve.
    /// </summary>
    /// <value>The types representing the number, order, and type of the parameters for the member to resolve.</value>
    public Type[] Types { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="StringComparison"/> rules to use when resolving a member name.
    /// </summary>
    /// <value>The <see cref="StringComparison"/> rules to use when resolving a member name.</value>
    public StringComparison Comparison { get; set; }
}
