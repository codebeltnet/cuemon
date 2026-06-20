---
uid: Cuemon.Extensions.Hosting.HostEnvironmentExtensions
example:
- *content
---

`HostEnvironmentExtensions` provides extension methods for `IHostEnvironment` to check whether the current environment is a local development or non-production environment. This example takes an `IHostEnvironment` parameter and calls `IsLocalDevelopment()` to detect a developer machine and `IsNonProduction()` to check for any non-production environment. Key setup includes using these methods in conditional startup logic. Console output prints `"Running on a developer machine."` or `"Environment is not Production."` based on the check results.

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
