---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationMvcOptionsSetup
example:
- *content
---

The following example demonstrates how to register <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationMvcOptionsSetup"/> with the dependency injection container to configure <xref cref="Microsoft.AspNetCore.Mvc.MvcOptions"/> with JSON serialization formatters, and resolves `IOptions<MvcOptions>` to verify the formatters are added.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.Configure<JsonFormatterOptions>(options =>
{
    options.Settings.WriteIndented = true;
});
services.AddTransient<IConfigureOptions<MvcOptions>, JsonSerializationMvcOptionsSetup>();

using var provider = services.BuildServiceProvider();
var mvcOptions = provider.GetRequiredService<IOptions<MvcOptions>>().Value;
Console.WriteLine($"Has JSON input formatter: {mvcOptions.InputFormatters.OfType<JsonSerializationInputFormatter>().Any()}");
Console.WriteLine($"Has JSON output formatter: {mvcOptions.OutputFormatters.OfType<JsonSerializationOutputFormatter>().Any()}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8c5c82211bc6089f411e1297"><a href="~/run-actions/8c5c82211bc6089f411e1297/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8c5c82211bc6089f411e1297/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
