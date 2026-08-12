using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
/// <summary>
/// A <see cref="ConfigureOptions{TOptions}"/> implementation which will add the XML serializer formatters to <see cref="MvcOptions"/>.
/// </summary>
public class XmlSerializationMvcOptionsSetup : ConfigureOptions<MvcOptions>
{
    /// <summary>
    /// Creates a new <see cref="XmlSerializationMvcOptionsSetup"/>.
    /// </summary>
    public XmlSerializationMvcOptionsSetup(IOptions<XmlFormatterOptions> formatterOptions) : base(mo =>
    {
        mo.OutputFormatters.Insert(0, new XmlSerializationOutputFormatter(formatterOptions?.Value));
        mo.InputFormatters.Insert(0, new XmlSerializationInputFormatter(formatterOptions?.Value));
    })
    {
    }
}
