---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcCoreBuilderExtensions
example:
- *content
---

The following example demonstrates how to register JSON serialization formatters on an <xref cref="Microsoft.AspNetCore.Mvc.IMvcCoreBuilder"/> using the <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcCoreBuilderExtensions"/> extension methods.

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace DocfxExamples;

public class MvcCoreBuilderExtensionsExample
{
    public void ConfigureMvc(IMvcCoreBuilder builder)
    {
        // Invoke the AddJsonFormatters extension method
        MvcCoreBuilderExtensions.AddJsonFormatters(builder, options =>
        {
            options.Settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        // Invoke the AddJsonFormattersOptions extension method
        MvcCoreBuilderExtensions.AddJsonFormattersOptions(builder, options =>
        {
            options.Settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

}
}

```
