using System.ComponentModel;
using System.Globalization;
using Cuemon.Text;

namespace Cuemon;
/// <summary>
/// Configuration options for <see cref="ParserFactory.FromObject"/> and methods of <see cref="ObjectDecoratorExtensions"/>.
/// </summary>
public class ObjectFormattingOptions : FormattingOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectFormattingOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="ObjectFormattingOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="FormattingOptions.FormatProvider"/></term>
    ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="DescriptorContext"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public ObjectFormattingOptions()
    {
    }

    /// <summary>
    /// Gets or sets the type specific format context.
    /// </summary>
    /// <value>The type specific format context.</value>
    public ITypeDescriptorContext DescriptorContext { get; set; }
}
