---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticator
example:
- *content
---

The following example demonstrates how to assign and invoke a <see cref="BasicAuthenticator" /> delegate when validating a basic-auth username and password pair.

```csharp
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;

namespace MyApp.Examples;

public static class BasicAuthenticatorExample
{
    public static void Demonstrate()
    {
        BasicAuthenticator authenticator = (username, password) =>
        {
            return username == "Agent" && password == "Test"
                ? new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, BasicAuthorizationHeader.Scheme))
                : null;
        };

        var principal = authenticator("Agent", "Test");

        Console.WriteLine(principal?.Identity?.Name);
    }
}
```
