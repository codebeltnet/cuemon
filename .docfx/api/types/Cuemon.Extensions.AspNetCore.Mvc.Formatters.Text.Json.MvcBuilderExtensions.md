---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcBuilderExtensions
example:
- *content
---

The following example demonstrates how to register JSON serialization formatters on an <xref cref="Microsoft.AspNetCore.Mvc.IMvcBuilder"/> using the <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcBuilderExtensions"/> extension methods.

```csharp
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace DocfxExamples;

public class MvcBuilderExtensionsExample
{
    public void ConfigureMvc(IMvcBuilder builder)
    {
        // Invoke the AddJsonFormatters extension method
        builder.AddJsonFormatters(options =>
        {
            options.SensitivityDetails = FaultSensitivityDetails.All;
            options.Settings.WriteIndented = true;
        });

        // Invoke the AddJsonFormattersOptions extension method
        builder.AddJsonFormattersOptions(options =>
        {
            options.Settings.WriteIndented = true;
        });

}
}

```
