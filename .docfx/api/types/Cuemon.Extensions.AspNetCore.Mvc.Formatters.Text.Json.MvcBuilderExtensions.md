---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcBuilderExtensions
example:
- *content
---

The following example demonstrates how to register JSON serialization formatters on an <xref cref="Microsoft.AspNetCore.Mvc.IMvcBuilder"/> using the <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.MvcBuilderExtensions"/> extension methods.

```csharp
// Program.cs
using System;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLogging();
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
Console.WriteLine("MVC JSON formatters configured.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="bd24fa00d3699a703f2e43f6"><a href="~/run-actions/bd24fa00d3699a703f2e43f6/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/bd24fa00d3699a703f2e43f6/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
