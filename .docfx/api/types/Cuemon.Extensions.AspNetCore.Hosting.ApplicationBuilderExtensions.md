---
uid: Cuemon.Extensions.AspNetCore.Hosting.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add the hosting environment HTTP header middleware to an ASP.NET Core application pipeline.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Hosting;
using Cuemon.Extensions.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddLogging();
        services.Configure<HostingEnvironmentOptions>(options =>
        {
            options.HeaderName = "X-Environment";
        });
    })
    .Build();

var app = new ApplicationBuilder(host.Services);
app.UseHostingEnvironment(options =>
{
    options.HeaderName = "X-Environment";
});

Console.WriteLine("Hosting environment middleware configured.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="bbb8a277e43674cb4922b1e0"><a href="~/run-actions/bbb8a277e43674cb4922b1e0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/bbb8a277e43674cb4922b1e0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
