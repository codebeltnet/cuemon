---
uid: Cuemon.Extensions.Hosting.HostEnvironmentExtensions
example:
- *content
---

The following example demonstrates how to use HostEnvironmentExtensions to check if the current hosting environment is a local development or non-production environment.

```csharp
using System;
using Cuemon.Extensions.Hosting;
using Microsoft.Extensions.Hosting;

namespace MyApp.Startup;

public class EnvironmentReporter
{
    public void Report(IHostEnvironment env)
    {
        if (env.IsLocalDevelopment())
        {
            Console.WriteLine("Running on a developer machine.");

        if (env.IsNonProduction())
        {
            Console.WriteLine("Environment is not Production.");

}}}
}

```
