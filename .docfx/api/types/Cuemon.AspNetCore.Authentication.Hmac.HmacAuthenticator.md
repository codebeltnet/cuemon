---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticator
example:
- *content
---

The following example demonstrates how to assign and invoke a <see cref="HmacAuthenticator" /> delegate for client-id and shared-secret lookup.

```csharp
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Hmac;

namespace MyApp.Examples;

public static class HmacAuthenticatorExample
{
    public static void Demonstrate()
    {
        HmacAuthenticator authenticator = (string clientId, out string clientSecret) =>
        {
            clientSecret = clientId == "Agent-Api" ? "Test" : null;
            return clientSecret == null
                ? null
                : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, clientId) }, HmacFields.Scheme));
        };

        var principal = authenticator("Agent-Api", out var secret);

        Console.WriteLine(secret);
        Console.WriteLine(principal?.Identity?.Name);
    }
}
```
