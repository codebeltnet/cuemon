---
uid: Cuemon.Extensions.AspNetCore.Authentication.AuthorizationResponseHandler
example:
- *content
---

The following example demonstrates how to construct `AuthorizationResponseHandler` with configured options and the required logger dependency. It registers `AuthorizationResponseHandlerOptions` with `FaultSensitivityDetails.All`, adds logging services, and builds the service provider. The handler is then created from the resolved `ILogger<T>` and `IOptions<T>` instances, and its configured sensitivity is written to the console, confirming the dependency-injection wiring works correctly.

```csharp
using System;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

public static class AuthorizationResponseHandlerExample
{
    public static void Demonstrate()
    {
        var services = new ServiceCollection();

        services.Configure<AuthorizationResponseHandlerOptions>(options =>
        {
            options.SensitivityDetails = Cuemon.Diagnostics.FaultSensitivityDetails.All;
        });

        services.AddLogging();

        using var provider = services.BuildServiceProvider();
        var logger = provider.GetRequiredService<ILogger<AuthorizationResponseHandler>>();
        var options = provider.GetRequiredService<IOptions<AuthorizationResponseHandlerOptions>>();
        var handler = new AuthorizationResponseHandler(logger, options);

        Console.WriteLine($"Configured fault sensitivity: {handler.Options.SensitivityDetails}");
    }
}

```
