---
uid: Cuemon.Extensions.Hosting.Environments
example:
- *content
---

The following example shows how to configure a .NET generic host to use the `Environments.LocalDevelopment` environment. The host is built and run with this environment setting.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder()
    .UseEnvironment(Cuemon.Extensions.Hosting.Environments.LocalDevelopment)
    .Build();

Console.WriteLine(host.Services.GetRequiredService<IHostEnvironment>().EnvironmentName);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8ddfd23fa92558e3b81d76b0"><a href="~/run-actions/8ddfd23fa92558e3b81d76b0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8ddfd23fa92558e3b81d76b0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
