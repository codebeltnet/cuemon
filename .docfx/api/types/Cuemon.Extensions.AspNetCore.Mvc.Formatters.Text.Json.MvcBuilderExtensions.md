---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcBuilderExtensions
example:
- *content
---

The following example demonstrates how to register JSON serialization formatters on an <xref cref="Microsoft.AspNetCore.Mvc.IMvcBuilder"/> using the <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcBuilderExtensions"/> extension methods and resolves `IOptions<MvcOptions>`, `IOptions<JsonFormatterOptions>`, and the JSON exception response formatter to verify the configuration.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddLogging();
services.AddFaultDescriptorOptions();
var builder = services.AddControllers();

builder.AddJsonFormatters(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.All;
    options.Settings.WriteIndented = true;
});

builder.AddJsonFormattersOptions(options =>
{
    options.Settings.WriteIndented = true;
});

using var provider = services.BuildServiceProvider();
var mvcOptions = provider.GetRequiredService<IOptions<MvcOptions>>().Value;
var jsonOptions = provider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;
var exceptionFormatter = provider.GetRequiredService<HttpExceptionDescriptorResponseFormatter<JsonFormatterOptions>>();

Console.WriteLine($"Has JSON input formatter: {mvcOptions.InputFormatters.OfType<JsonSerializationInputFormatter>().Any()}");
Console.WriteLine($"Has JSON output formatter: {mvcOptions.OutputFormatters.OfType<JsonSerializationOutputFormatter>().Any()}");
Console.WriteLine($"WriteIndented: {jsonOptions.Settings.WriteIndented}, SensitivityDetails: {jsonOptions.SensitivityDetails}");
Console.WriteLine($"Exception formatter resolved: {exceptionFormatter is not null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="cdd1306d830e95ebd9278e13"><a href="~/run-actions/cdd1306d830e95ebd9278e13/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/cdd1306d830e95ebd9278e13/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
