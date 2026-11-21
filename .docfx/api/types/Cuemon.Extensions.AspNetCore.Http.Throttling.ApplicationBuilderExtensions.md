---
uid: Cuemon.Extensions.AspNetCore.Http.Throttling.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add a request rate limiting middleware to an ASP.NET Core application pipeline.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLogging();
services.AddMemoryThrottlingCache();
services.AddThrottlingSentinelOptions(options =>
{
    options.Quota = new ThrottleQuota(100, 1, TimeUnit.Minutes);
    options.ContextResolver = context => context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
});

using var provider = services.BuildServiceProvider();
var app = new ApplicationBuilder(provider);
app.UseThrottlingSentinel(options =>
{
    options.Quota = new ThrottleQuota(100, 1, TimeUnit.Minutes);
    options.ContextResolver = context => context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
});

Console.WriteLine("Throttling sentinel middleware configured.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="08aec9f49e43cb171e73f34c"><a href="~/run-actions/08aec9f49e43cb171e73f34c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/08aec9f49e43cb171e73f34c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
