using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Extensions.AspNetCore.Text.Json.Converters;
using Cuemon.Extensions.Text.Json.Formatters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
/// <summary>
/// This class handles deserialization of JSON to objects using <see cref="JsonFormatter"/>.
/// </summary>
public class JsonSerializationInputFormatter : StreamInputFormatter<JsonFormatter, JsonFormatterOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonSerializationInputFormatter" /> class.
    /// </summary>
    /// <param name="options">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
    public JsonSerializationInputFormatter(JsonFormatterOptions options) : base(options)
    {
        options.Settings.Converters.AddHttpExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails);
        foreach (var mediaType in options.SupportedMediaTypes)
        {
            SupportedMediaTypes.Add(mediaType.ToString());
        }
    }
}
