---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthorizationHeader
example:
- *content
---

The following example demonstrates how to serialize and parse a Digest access authentication header.

```csharp
using System;
using Cuemon.AspNetCore.Authentication.Digest;

namespace MyApp.Examples;

public static class DigestAuthorizationHeaderExample
{
    public static void Demonstrate()
    {
        var header = new DigestAuthorizationHeader(
            realm: "docs-example",
            nonce: "abc123",
            opaque: "opaque456",
            algorithm: "SHA-256",
            userName: "Agent",
            uri: "/resource",
            nc: "00000001",
            cNonce: "client-nonce",
            qop: "auth",
            response: "deadbeef");

        var parsed = DigestAuthorizationHeader.Create(header.ToString());

        Console.WriteLine(parsed.UserName);
        Console.WriteLine(parsed.Response);
    }
}

```
