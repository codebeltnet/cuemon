using Cuemon.Configuration;

namespace Cuemon.Text;
/// <summary>
/// Configuration options for <see cref="ParserFactory.FromGuid"/>.
/// </summary>
/// <seealso cref="IParameterObject"/>
public class GuidStringOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GuidStringOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="GuidStringOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Formats"/></term>
    ///         <description><c>GuidFormats.BraceFormat | GuidFormats.DigitFormat | GuidFormats.ParenthesisFormat</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public GuidStringOptions()
    {
        Formats = GuidFormats.B | GuidFormats.D | GuidFormats.P;
    }

    /// <summary>
    /// Gets or sets the allowed GUID formats.
    /// </summary>
    /// <value>The allowed GUID formats.</value>
    public GuidFormats Formats { get; set; }
}
