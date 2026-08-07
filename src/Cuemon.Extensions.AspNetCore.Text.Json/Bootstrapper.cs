using System.Threading;
using Cuemon.Extensions.AspNetCore.Text.Json.Converters;
using Cuemon.Extensions.Text.Json.Formatters;

namespace Cuemon.Extensions.AspNetCore.Text.Json;
internal static class Bootstrapper
{
    private static readonly Lock PadLock = new();
    private static bool _initialized;

    internal static void Initialize()
    {
        if (!_initialized)
        {
            lock (PadLock)
            {
                if (!_initialized)
                {
                    _initialized = true;
                    JsonFormatterOptions.DefaultConverters += list =>
                    {
                        list.AddStringValuesConverter();
                        list.AddProblemDetailsConverter();
                        list.AddHeaderDictionaryConverter();
                    };
                }
            }
        }
    }
}
