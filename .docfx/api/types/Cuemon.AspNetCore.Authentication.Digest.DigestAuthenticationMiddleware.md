---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationMiddleware
example:
- *content
---

The following example demonstrates how to construct `DigestAuthenticationMiddleware` with inline option setup.

```csharp
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Digest;

namespace MyApp.Examples;

public static class DigestAuthenticationMiddlewareExample
{
    public static void Demonstrate()
    {
        var middleware = new DigestAuthenticationMiddleware(_ => Task.CompletedTask, options =>
        {
            options.Realm = "docs-example";
            options.RequireSecureConnection = false;
            options.Authenticator = (string username, out string password) =>
            {
                password = username == "Agent" ? "Test" : null;
                return password == null ? null : new ClaimsPrincipal(new ClaimsIdentity());
            };
        });

        Console.WriteLine(middleware.Options.Realm);
    }
}

```
