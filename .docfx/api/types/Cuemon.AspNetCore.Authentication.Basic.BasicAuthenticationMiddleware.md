---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationMiddleware
example:
- *content
---

The following example demonstrates how to construct `BasicAuthenticationMiddleware` with inline option setup.

```csharp
using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Basic;

namespace MyApp.Examples;

public static class BasicAuthenticationMiddlewareExample
{
    public static void Demonstrate()
    {
        var middleware = new BasicAuthenticationMiddleware(_ => Task.CompletedTask, options =>
        {
            options.Realm = "docs-example";
            options.RequireSecureConnection = false;
            options.Authenticator = (username, password) => null;
        });

        Console.WriteLine(middleware.Options.Realm);
    }
}

```
