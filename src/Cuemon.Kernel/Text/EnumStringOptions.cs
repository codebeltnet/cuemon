using System;
using Cuemon.Configuration;

namespace Cuemon.Text;
/// <summary>
/// Configuration options for concrete implementations of <see cref="IConfigurableParser{TOptions}"/> that is related to parsing an <see cref="Enum"/> from a <see cref="string"/>.
/// </summary>
/// <seealso cref="IParameterObject"/>
public class EnumStringOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumStringOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="EnumStringOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="IgnoreCase"/></term>
    ///         <description><c>true</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public EnumStringOptions()
    {
        IgnoreCase = true;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore or regard the case of the string being parsed.
    /// </summary>
    /// <value><c>true</c> to ignore the case of the string being parsed; otherwise, <c>false</c>.</value>
    public bool IgnoreCase { get; set; }
}
