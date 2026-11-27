---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcCoreBuilderExtensions
example:
- *content
---

The following example demonstrates how to register JSON serialization formatters on an <xref cref="Microsoft.AspNetCore.Mvc.IMvcCoreBuilder"/> using the <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcCoreBuilderExtensions"/> extension methods and resolves `IOptions<MvcOptions>`, `IOptions<JsonFormatterOptions>`, and the JSON exception response formatter to verify the configuration.

```csharp
// Program.cs
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddLogging();
services.AddFaultDescriptorOptions();
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
var mvcOptions = provider.GetRequiredService<IOptions<MvcOptions>>().Value;
var jsonOptions = provider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;
var exceptionFormatter = provider.GetRequiredService<HttpExceptionDescriptorResponseFormatter<JsonFormatterOptions>>();

Console.WriteLine($"Has JSON input formatter: {mvcOptions.InputFormatters.OfType<JsonSerializationInputFormatter>().Any()}");
Console.WriteLine($"Has JSON output formatter: {mvcOptions.OutputFormatters.OfType<JsonSerializationOutputFormatter>().Any()}");
Console.WriteLine($"NamingPolicy: {jsonOptions.Settings.PropertyNamingPolicy?.GetType().Name}, DefaultIgnoreCondition: {jsonOptions.Settings.DefaultIgnoreCondition}");
Console.WriteLine($"Exception formatter resolved: {exceptionFormatter is not null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="bd5a07aa1fec06a7b5ac17bd"><a href="~/run-actions/bd5a07aa1fec06a7b5ac17bd/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/bd5a07aa1fec06a7b5ac17bd/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
