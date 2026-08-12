using System.Runtime.CompilerServices;
using Cuemon.Extensions.AspNetCore.Text.Json.Converters;
using Cuemon.Extensions.Text.Json.Converters;
using Cuemon.Extensions.Text.Json.Formatters;

namespace Cuemon.AspNetCore;
static class Bootstrapper
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        JsonFormatterOptions.DefaultConverters += o =>
        {
            o.AddHeaderDictionaryConverter();
            o.AddDateTimeConverter();
        };
    }
}
