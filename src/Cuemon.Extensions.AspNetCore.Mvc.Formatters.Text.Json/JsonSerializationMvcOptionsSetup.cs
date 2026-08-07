using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
/// <summary>
/// A <see cref="ConfigureOptions{TOptions}"/> implementation which will add the JSON serializer formatters to <see cref="MvcOptions"/>.
/// </summary>
public class JsonSerializationMvcOptionsSetup : ConfigureOptions<MvcOptions>
{
    /// <summary>
    /// Creates a new <see cref="JsonSerializationMvcOptionsSetup"/>.
    /// </summary>
    public JsonSerializationMvcOptionsSetup(IOptions<JsonFormatterOptions> formatterOptions) : base(mo =>
    {
        mo.OutputFormatters.Insert(0, new JsonSerializationOutputFormatter(formatterOptions?.Value));
        mo.InputFormatters.Insert(0, new JsonSerializationInputFormatter(formatterOptions?.Value));
    })
    {
    }
}
