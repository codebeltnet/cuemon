using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Extensions.AspNetCore.Xml.Converters;
using Cuemon.Xml.Serialization.Formatters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
/// <summary>
/// This class handles deserialization of XML to objects using <see cref="XmlFormatter"/>.
/// </summary>
public class XmlSerializationInputFormatter : StreamInputFormatter<XmlFormatter, XmlFormatterOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlSerializationInputFormatter" /> class.
    /// </summary>
    /// <param name="options">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
    public XmlSerializationInputFormatter(XmlFormatterOptions options) : base(options)
    {
        options.Settings.Converters.AddHttpExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails);
        foreach (var mediaType in options.SupportedMediaTypes)
        {
            SupportedMediaTypes.Add(mediaType.ToString());
        }
    }
}
