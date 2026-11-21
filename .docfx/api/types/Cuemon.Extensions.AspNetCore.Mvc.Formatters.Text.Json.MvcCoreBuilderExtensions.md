---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcCoreBuilderExtensions
example:
- *content
---

The following example demonstrates how to register JSON serialization formatters on an <xref cref="Microsoft.AspNetCore.Mvc.IMvcCoreBuilder"/> using the <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcCoreBuilderExtensions"/> extension methods.

```csharp
// Program.cs
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLogging();
var builder = services.AddMvcCore();

builder.AddJsonFormatters(options =>
{
    options.Settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.Settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.AddJsonFormattersOptions(options =>
{
    options.Settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

using var provider = services.BuildServiceProvider();
Console.WriteLine("MVC Core JSON formatters configured.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="439d502fc2a2ac0ece7c9a8a"><a href="~/run-actions/439d502fc2a2ac0ece7c9a8a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/439d502fc2a2ac0ece7c9a8a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
