---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationMvcOptionsSetup
example:
- *content
---

The following example demonstrates how to register <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.JsonSerializationMvcOptionsSetup"/> with the dependency injection container to configure <xref cref="Microsoft.AspNetCore.Mvc.MvcOptions"/> with JSON serialization formatters.

```csharp
// Program.cs
using System;
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
var setup = provider.GetService<IConfigureOptions<MvcOptions>>();
Console.WriteLine(setup is JsonSerializationMvcOptionsSetup);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9f8b3201ad4fd814cf0c92a1"><a href="~/run-actions/9f8b3201ad4fd814cf0c92a1/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9f8b3201ad4fd814cf0c92a1/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
