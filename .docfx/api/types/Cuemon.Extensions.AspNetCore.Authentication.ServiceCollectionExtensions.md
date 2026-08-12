---
uid: Cuemon.Extensions.AspNetCore.Authentication.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register the in-memory digest nonce tracker and the authorization response handler services. It inserts and reads a nonce to verify the tracker registration, then reports the configured fault-sensitivity option used by the authorization response handler.

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

        var tracker = provider.GetRequiredService<INonceTracker>();
        var added = tracker.TryAddEntry("docs-nonce", 1);
        var found = tracker.TryGetEntry("docs-nonce", out var entry);
        var responseHandler = provider.GetRequiredService<AuthorizationResponseHandler>();

        Console.WriteLine($"Nonce added: {added}; found: {found}; count: {entry?.Count}");
        Console.WriteLine($"Configured fault sensitivity: {responseHandler.Options.SensitivityDetails}");
    }
}

```
