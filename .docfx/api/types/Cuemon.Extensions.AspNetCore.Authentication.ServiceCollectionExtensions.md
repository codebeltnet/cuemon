---
uid: Cuemon.Extensions.AspNetCore.Authentication.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register the in-memory digest nonce tracker and the authorization response handler services.

```csharp
using System;
using Cuemon.AspNetCore.Authentication;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

public static class ServiceCollectionExtensionsExample
{
    public static void Demonstrate()
    {
        var services = new ServiceCollection();

        services.AddInMemoryDigestAuthenticationNonceTracker();
        services.AddAuthorizationResponseHandler(o =>
        {
            o.SensitivityDetails = FaultSensitivityDetails.All;
        });

        using var provider = services.BuildServiceProvider();

        Console.WriteLine(provider.GetRequiredService<INonceTracker>().GetType().Name);
        Console.WriteLine(provider.GetRequiredService<AuthorizationResponseHandler>().GetType().Name);
    }
}

```
