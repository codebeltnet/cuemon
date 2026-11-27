---
uid: Cuemon.Extensions.AspNetCore.Text.Json.Formatters.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register JSON formatter options and a JSON-based exception response formatter in the ASP.NET Core service collection, then resolves both from the service provider.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddFaultDescriptorOptions();
services.AddJsonFormatterOptions(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.All;
});
services.AddJsonExceptionResponseFormatter();

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;
var formatter = provider.GetRequiredService<HttpExceptionDescriptorResponseFormatter<JsonFormatterOptions>>();

Console.WriteLine(options.SensitivityDetails);
Console.WriteLine($"Formatter resolved: {formatter is not null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c25980c10e256f16a38a88af"><a href="~/run-actions/c25980c10e256f16a38a88af/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c25980c10e256f16a38a88af/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
