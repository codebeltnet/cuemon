---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationOptions
example:
- *content
---

The following example demonstrates how to configure `DigestAuthenticationOptions` with a realm, digest algorithm, and authenticator delegate.

```csharp
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Digest;

namespace MyApp.Examples;

public static class DigestAuthenticationOptionsExample
{
    public static void Demonstrate()
    {
        var options = new DigestAuthenticationOptions
        {
            DigestAlgorithm = DigestCryptoAlgorithm.Sha256,
            Realm = "docs-example",
            RequireSecureConnection = false,
            Authenticator = (string username, out string password) =>
            {
                password = username == "Agent" ? "Test" : null;
                return password == null
                    ? null
                    : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, DigestAuthorizationHeader.Scheme));
            }
        };

        options.ValidateOptions();

        Console.WriteLine(options.DigestAlgorithm);
    }
}

```
