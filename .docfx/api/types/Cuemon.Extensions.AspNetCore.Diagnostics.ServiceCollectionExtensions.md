---
uid: Cuemon.Extensions.AspNetCore.Diagnostics.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.AspNetCore.Diagnostics.ServiceCollectionExtensions> extension methods to configure diagnostics and fault handling in an ASP.NET Core application.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddServerTiming(options =>
{
    options.TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(10);
});

services.AddFaultDescriptorOptions(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.All;
    options.RootHelpLink = new Uri("https://example.com/help");
});

services.AddExceptionDescriptorOptions(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.None;
});

services.AddServerTimingOptions(options =>
{
    options.TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(50);
});

services.PostConfigureAllExceptionDescriptorOptions(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace;
});

using var provider = services.BuildServiceProvider();
var serverTiming = provider.GetRequiredService<IOptions<ServerTimingOptions>>().Value;
var fault = provider.GetRequiredService<IOptions<FaultDescriptorOptions>>().Value;
var exception = provider.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value;

Console.WriteLine(serverTiming.TimeMeasureCompletedThreshold.TotalMilliseconds);
Console.WriteLine(fault.SensitivityDetails);
Console.WriteLine(exception.SensitivityDetails);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="20db73386a90b6c65197f9ac"><a href="~/run-actions/20db73386a90b6c65197f9ac/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/20db73386a90b6c65197f9ac/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
