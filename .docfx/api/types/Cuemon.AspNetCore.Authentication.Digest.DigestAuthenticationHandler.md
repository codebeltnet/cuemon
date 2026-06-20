---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationHandler
example:
- *content
---

The following example demonstrates how to register `DigestAuthenticationHandler` with ASP.NET Core authentication services. It configures DI with `INonceTracker`, registers a digest authentication scheme with a username/password lookup callback, and builds the service provider. The handler is resolved from DI and its type name is written to the console, verifying that digest authentication wiring is operational.

```csharp
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication;
using Cuemon.AspNetCore.Authentication.Digest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

public static class DigestAuthenticationHandlerExample
{
    public static void Demonstrate()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddSingleton<INonceTracker, MemoryNonceTracker>();
        services.AddAuthentication(DigestAuthorizationHeader.Scheme)
            .AddScheme<DigestAuthenticationOptions, DigestAuthenticationHandler>(DigestAuthorizationHeader.Scheme, options =>
            {
                options.Realm = "docs-example";
                options.RequireSecureConnection = false;
                options.Authenticator = (string username, out string password) =>
                {
                    password = username == "Agent" ? "Test" : null;
                    return password == null
                        ? null
                        : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, DigestAuthorizationHeader.Scheme));
                };
            });

        using var provider = services.BuildServiceProvider();
        var handler = provider.GetRequiredService<DigestAuthenticationHandler>();

        Console.WriteLine(handler.GetType().Name);
    }
}

```
