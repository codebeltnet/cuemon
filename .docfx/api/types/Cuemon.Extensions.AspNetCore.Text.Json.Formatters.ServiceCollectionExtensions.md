---
uid: Cuemon.Extensions.AspNetCore.Text.Json.Formatters.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register JSON formatter options and a JSON-based exception response formatter in the ASP.NET Core service collection.

```csharp
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

public class JsonFormattersServiceCollectionExtensionsExample
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddJsonFormatterOptions(options =>
        {
            options.SensitivityDetails = Cuemon.Diagnostics.FaultSensitivityDetails.All;
        });

        services.AddJsonExceptionResponseFormatter();

}
}

```
