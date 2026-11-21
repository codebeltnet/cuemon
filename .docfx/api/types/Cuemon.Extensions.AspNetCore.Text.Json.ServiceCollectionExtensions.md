---
uid: Cuemon.Extensions.AspNetCore.Text.Json.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register minimal JSON formatter options using `AddMinimalJsonOptions` in the ASP.NET Core service collection.

```csharp
// Program.cs
using System;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Json;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.Configure<JsonFormatterOptions>(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.All;
});
services.AddMinimalJsonOptions(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.All;
});

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;
Console.WriteLine(options.SensitivityDetails);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="23f73e719e754b425fa1c58f"><a href="~/run-actions/23f73e719e754b425fa1c58f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/23f73e719e754b425fa1c58f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
