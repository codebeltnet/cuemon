using Cuemon.Text;

namespace Cuemon.Xml;
/// <summary>
/// Configuration options for <see cref="XmlEncodingOptions"/>.
/// </summary>
public class XmlEncodingOptions : EncodingOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlEncodingOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="XmlEncodingOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="EncodingOptions.Preamble"/></term>
    ///         <description><see cref="EncodingOptions.DefaultPreambleSequence"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="EncodingOptions.Encoding"/></term>
    ///         <description><see cref="EncodingOptions.DefaultEncoding"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="OmitXmlDeclaration"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public XmlEncodingOptions()
    {
        OmitXmlDeclaration = false;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to omit an XML declaration.
    /// </summary>
    /// <returns>
    /// <see langword="true" /> to omit the XML declaration; otherwise, <see langword="false" />. The default is <see langword="false" />, an XML declaration is written.
    /// </returns>
    public bool OmitXmlDeclaration { get; set; }
}
