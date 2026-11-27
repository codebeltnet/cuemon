---
uid: Cuemon.Extensions.Hosting.HostBuilderExtensions
example:
- *content
---

The following example demonstrates how to add and remove configuration sources through <see cref="HostBuilderExtensions" />.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Hosting;

var hostBuilder = Host.CreateDefaultBuilder()
    .ConfigureConfigurationSources((environment, sources) =>
    {
        sources.Add(new MemoryConfigurationSource
        {
            InitialData = new Dictionary<string, string>
            {
                ["App:Environment"] = environment.EnvironmentName
            }
        });
    })
    .RemoveConfigurationSource((environment, source) =>
        environment.IsProduction() && source is MemoryConfigurationSource);

using var host = hostBuilder.Build();
var configuration = (IConfiguration)host.Services.GetService(typeof(IConfiguration));

Console.WriteLine(configuration["App:Environment"] == null);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d9b359d65ea80b03a9e6073d"><a href="~/run-actions/d9b359d65ea80b03a9e6073d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d9b359d65ea80b03a9e6073d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
