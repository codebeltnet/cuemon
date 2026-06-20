---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticator
example:
- *content
---

The following example demonstrates how to assign and invoke a <see cref="DigestAuthenticator" /> delegate when a digest-auth username resolves to a stored password.

```csharp
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Digest;

namespace MyApp.Examples;

public static class DigestAuthenticatorExample
{
    public static void Demonstrate()
    {
        DigestAuthenticator authenticator = (string username, out string password) =>
        {
            password = username == "Agent" ? "Test" : null;
            return password == null
                ? null
                : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, DigestAuthorizationHeader.Scheme));
        };

        var principal = authenticator("Agent", out var password);

        Console.WriteLine(password);
        Console.WriteLine(principal?.Identity?.Name);
    }
}
```
