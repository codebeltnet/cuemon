---
uid: Cuemon.Extensions.Hosting.HostBuilderExtensions
example:
- *content
---

The following example demonstrates how to add and remove configuration sources through <see cref="HostBuilderExtensions" />.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Hosting;

namespace MyApp.Examples;

public static class HostBuilderExtensionsExample
{
    public static void Demonstrate()
    {
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
    }
}
```
