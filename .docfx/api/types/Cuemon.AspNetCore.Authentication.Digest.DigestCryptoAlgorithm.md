---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestCryptoAlgorithm
example:
- *content
---

The following example demonstrates how `DigestCryptoAlgorithm` selects the hash algorithm used by `DigestAuthorizationHeaderBuilder`.

```csharp
using System;
using Cuemon.AspNetCore.Authentication.Digest;

namespace MyApp.Examples;

public static class DigestCryptoAlgorithmExample
{
    public static void Demonstrate()
    {
        var builder = new DigestAuthorizationHeaderBuilder(DigestCryptoAlgorithm.Sha512Slash256);

        Console.WriteLine(builder.DigestAlgorithm);
    }
}

```
