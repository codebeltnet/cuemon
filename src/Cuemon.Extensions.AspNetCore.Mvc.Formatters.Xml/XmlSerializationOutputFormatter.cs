using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Extensions.AspNetCore.Xml.Converters;
using Cuemon.Xml.Serialization.Formatters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
/// <summary>
/// This class handles serialization of objects to XML using <see cref="XmlFormatter"/>.
/// </summary>
public class XmlSerializationOutputFormatter : StreamOutputFormatter<XmlFormatter, XmlFormatterOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlSerializationOutputFormatter"/> class.
    /// </summary>
    /// <param name="options">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
    public XmlSerializationOutputFormatter(XmlFormatterOptions options) : base(options)
    {
        options.Settings.Converters.AddHttpExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails);
        foreach (var mediaType in options.SupportedMediaTypes)
        {
            SupportedMediaTypes.Add(mediaType.ToString());
        }
    }
}
