---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationMvcOptionsSetup
example:
- *content
---

The following example demonstrates how to register <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationMvcOptionsSetup"/> with the dependency injection container to configure <xref cref="Microsoft.AspNetCore.Mvc.MvcOptions"/> with JSON serialization formatters.

```csharp
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using System;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DocfxExamples;

public class JsonSerializationMvcOptionsSetupExample
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<JsonFormatterOptions>(options =>
        {
            options.Settings.WriteIndented = true;
        });

        services.AddTransient<IConfigureOptions<MvcOptions>, JsonSerializationMvcOptionsSetup>();

        var serviceProvider = services.BuildServiceProvider();
        var setup = serviceProvider.GetService<IConfigureOptions<MvcOptions>>();
        Console.WriteLine(setup is JsonSerializationMvcOptionsSetup); // True

}
}

```
