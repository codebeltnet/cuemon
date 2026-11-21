---
uid: Cuemon.Extensions.AspNetCore.Text.Json.Formatters.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register JSON formatter options and a JSON-based exception response formatter in the ASP.NET Core service collection.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddJsonFormatterOptions(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.All;
});
services.AddJsonExceptionResponseFormatter();

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;
var formatters = provider.GetServices<IHttpExceptionDescriptorResponseFormatter>().ToList();

Console.WriteLine(options.SensitivityDetails);
Console.WriteLine(formatters.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7bf1e6aaa5809ee1009e5d94"><a href="~/run-actions/7bf1e6aaa5809ee1009e5d94/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7bf1e6aaa5809ee1009e5d94/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
