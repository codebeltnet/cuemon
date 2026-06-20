---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationOptions
example:
- *content
---

The following example demonstrates how to configure `HmacAuthenticationOptions` for HMAC request signing.

```csharp
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Hmac;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class HmacAuthenticationOptionsExample
{
    public static void Demonstrate()
    {
        var options = new HmacAuthenticationOptions
        {
            AuthenticationScheme = "hmac-docs",
            Algorithm = KeyedCryptoAlgorithm.HmacSha256,
            RequireSecureConnection = false,
            Authenticator = (string clientId, out string clientSecret) =>
            {
                clientSecret = clientId == "Agent-Api" ? "Test" : null;
                return clientSecret == null
                    ? null
                    : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, clientId) }, "hmac-docs"));
            }
        };

        options.ValidateOptions();

        Console.WriteLine(options.AuthenticationScheme);
    }
}

```
