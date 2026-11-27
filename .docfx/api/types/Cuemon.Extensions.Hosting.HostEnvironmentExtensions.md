---
uid: Cuemon.Extensions.Hosting.HostEnvironmentExtensions
example:
- *content
---

`HostEnvironmentExtensions` provides extension methods for `IHostEnvironment` to check whether the current environment is a local development or non-production environment. This example takes an `IHostEnvironment` parameter and calls `IsLocalDevelopment()` to detect a developer machine and `IsNonProduction()` to check for any non-production environment. Key setup includes using these methods in conditional startup logic. Console output prints `"Running on a developer machine."` or `"Environment is not Production."` based on the check results.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder()
    .UseEnvironment(Cuemon.Extensions.Hosting.Environments.LocalDevelopment)
    .Build();

var environment = host.Services.GetRequiredService<IHostEnvironment>();
if (environment.IsLocalDevelopment())
{
    Console.WriteLine("Running on a developer machine.");
}
if (environment.IsNonProduction())
{
    Console.WriteLine("Environment is not Production.");
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4fecaba32bc54a3b651f9273"><a href="~/run-actions/4fecaba32bc54a3b651f9273/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4fecaba32bc54a3b651f9273/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
