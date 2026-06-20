---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthorizationHeaderBuilder
example:
- *content
---

The following example demonstrates how to build a Digest access authorization header from a challenge header.

```csharp
using System;
using Cuemon.AspNetCore.Authentication.Digest;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace MyApp.Examples;

public static class DigestAuthorizationHeaderBuilderExample
{
    public static void Demonstrate()
    {
        var headers = new HeaderDictionary
        {
            [HeaderNames.WWWAuthenticate] = "Digest realm=\"docs-example\", qop=\"auth, auth-int\", nonce=\"abc123\", opaque=\"opaque456\", stale=false, algorithm=SHA-256"
        };

        var builder = new DigestAuthorizationHeaderBuilder(DigestCryptoAlgorithm.Sha256)
            .AddRealm("docs-example")
            .AddUserName("Agent")
            .AddUri("/resource")
            .AddNc(1)
            .AddCnonce("client-nonce")
            .AddQopAuthentication()
            .AddFromWwwAuthenticateHeader(headers)
            .AddResponse("Test", "GET");

        var header = builder.Build();

        Console.WriteLine(header.ToString());
    }
}

```
