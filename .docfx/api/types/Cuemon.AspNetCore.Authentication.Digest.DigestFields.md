---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestFields
example:
- *content
---

The following example demonstrates how to use the field constants of `DigestFields` when building a Digest access authorization header.

```csharp
using System;
using Cuemon.AspNetCore.Authentication.Digest;

namespace MyApp.Examples;

public static class DigestFieldsExample
{
    public static void Demonstrate()
    {
        var builder = new DigestAuthorizationHeaderBuilder(DigestCryptoAlgorithm.Sha256);
        builder.AddRealm("my-realm");
        builder.AddUserName("alice");
        builder.AddUri("/api/resource");
        builder.AddNc(1);
        builder.AddCnonce(Guid.NewGuid().ToString("N"));

        Console.WriteLine(DigestFields.Realm);
        Console.WriteLine(DigestFields.Nonce);
        Console.WriteLine(DigestFields.QualityOfProtection);
    }
}

```
