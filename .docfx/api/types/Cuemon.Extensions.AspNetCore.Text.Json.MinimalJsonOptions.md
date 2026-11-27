---
uid: Cuemon.Extensions.AspNetCore.Text.Json.MinimalJsonOptions
example:
- *content
---

The following example demonstrates how to register <see cref="Cuemon.Extensions.AspNetCore.Text.Json.MinimalJsonOptions"/> in an ASP.NET Core application to propagate custom <see cref="T:Cuemon.Extensions.Text.Json.Formatters.JsonFormatterOptions"/> into the minimal API JSON serialization pipeline.

```csharp
// Program.cs
using System;
using System.Text.Json;
using Cuemon.Extensions.AspNetCore.Text.Json;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();

// Configure JsonFormatterOptions with custom settings
services.Configure<JsonFormatterOptions>(o =>
{
    o.Settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    o.Settings.WriteIndented = false;
});

// Register MinimalJsonOptions so that JsonOptions (minimal API)
// receives the same settings and converters
services.AddTransient<IConfigureOptions<JsonOptions>, MinimalJsonOptions>();

var provider = services.BuildServiceProvider();
var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>();

Console.WriteLine($"Property naming policy: {jsonOptions.Value.SerializerOptions.PropertyNamingPolicy}"); // CamelCase
Console.WriteLine($"Write indented: {jsonOptions.Value.SerializerOptions.WriteIndented}");               // False
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a6d2e7f0a4c03c8dee411474"><a href="~/run-actions/a6d2e7f0a4c03c8dee411474/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a6d2e7f0a4c03c8dee411474/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
