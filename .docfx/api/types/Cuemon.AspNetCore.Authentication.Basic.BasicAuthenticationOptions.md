---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationOptions
example:
- *content
---

The following example demonstrates how to configure `BasicAuthenticationOptions` with a realm and authenticator delegate.

```csharp
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;

namespace MyApp.Examples;

public static class BasicAuthenticationOptionsExample
{
    public static void Demonstrate()
    {
        var options = new BasicAuthenticationOptions
        {
            Realm = "docs-example",
            RequireSecureConnection = false,
            Authenticator = (username, password) =>
            {
                if (username == "Agent" && password == "Test")
                {
                    return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, BasicAuthorizationHeader.Scheme));
                }

                return null;
            }
        };

        options.ValidateOptions();

        Console.WriteLine(options.Realm);
    }
}

```
