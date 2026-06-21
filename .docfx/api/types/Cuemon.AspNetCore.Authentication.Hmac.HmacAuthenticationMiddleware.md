---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationMiddleware
example:
- *content
---

The following example demonstrates how to construct `HmacAuthenticationMiddleware` with inline option setup.

```csharp
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Hmac;

namespace MyApp.Examples;

public static class HmacAuthenticationMiddlewareExample
{
    public static void Demonstrate()
    {
        var middleware = new HmacAuthenticationMiddleware(_ => Task.CompletedTask, options =>
        {
            options.AuthenticationScheme = "hmac-docs";
            options.RequireSecureConnection = false;
            options.Authenticator = (string clientId, out string clientSecret) =>
            {
                clientSecret = clientId == "Agent-Api" ? "Test" : null;
                return clientSecret == null ? null : new ClaimsPrincipal(new ClaimsIdentity());
            };
        });

        Console.WriteLine(middleware.Options.AuthenticationScheme);
    }
}

```
