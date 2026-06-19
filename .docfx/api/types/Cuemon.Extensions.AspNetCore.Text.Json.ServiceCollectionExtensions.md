---
uid: Cuemon.Extensions.AspNetCore.Text.Json.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register minimal JSON formatter options using `AddMinimalJsonOptions` in the ASP.NET Core service collection.

```csharp
using Cuemon.Extensions.AspNetCore.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

public class TextJsonServiceCollectionExtensionsExample
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMinimalJsonOptions(options =>
        {
            options.SensitivityDetails = Cuemon.Diagnostics.FaultSensitivityDetails.All;
        });

}
}

```
