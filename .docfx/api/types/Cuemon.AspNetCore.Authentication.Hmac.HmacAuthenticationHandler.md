---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationHandler
example:
- *content
---

The following example demonstrates how to register `HmacAuthenticationHandler` with ASP.NET Core authentication services.

```csharp
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Hmac;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

public static class HmacAuthenticationHandlerExample
{
    public static void Demonstrate()
    {
        const string authenticationScheme = "hmac-docs";
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddAuthentication(authenticationScheme)
            .AddScheme<HmacAuthenticationOptions, HmacAuthenticationHandler>(authenticationScheme, options =>
            {
                options.AuthenticationScheme = authenticationScheme;
                options.RequireSecureConnection = false;
                options.Authenticator = (string clientId, out string clientSecret) =>
                {
                    clientSecret = clientId == "Agent-Api" ? "Test" : null;
                    return clientSecret == null
                        ? null
                        : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, clientId) }, authenticationScheme));
                };
            });

        using var provider = services.BuildServiceProvider();
        var handler = provider.GetRequiredService<HmacAuthenticationHandler>();

        Console.WriteLine(handler.GetType().Name);
    }
}

```
