---
uid: Cuemon.Extensions.AspNetCore.Text.Json.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register minimal JSON formatter options using `AddMinimalJsonOptions` in the ASP.NET Core service collection and resolves the configured `JsonFormatterOptions`, `JsonOptions`, and JSON exception response formatter.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Json;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddFaultDescriptorOptions();
services.AddMinimalJsonOptions(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.All;
});

using var provider = services.BuildServiceProvider();
var formatterOptions = provider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;
var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;
var exceptionFormatter = provider.GetRequiredService<HttpExceptionDescriptorResponseFormatter<JsonFormatterOptions>>();

Console.WriteLine(formatterOptions.SensitivityDetails);
Console.WriteLine($"JsonOptions converters: {jsonOptions.SerializerOptions.Converters.Count}");
Console.WriteLine($"Exception formatter resolved: {exceptionFormatter is not null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="fb3f4680cb00248f5083c71b"><a href="~/run-actions/fb3f4680cb00248f5083c71b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/fb3f4680cb00248f5083c71b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
