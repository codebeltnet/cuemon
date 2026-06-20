---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestHashFactory
example:
- *content
---

The following example demonstrates how to create a cryptographic hash instance using `DigestHashFactory` with a specified algorithm.

```csharp
using System;
using System.Text;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.Security;

namespace MyApp.Examples;

public static class DigestHashFactoryExample
{
    public static void Demonstrate()
    {
        Hash hash = DigestHashFactory.CreateCrypto(DigestCryptoAlgorithm.Sha256);

        var bytes = Encoding.UTF8.GetBytes("hello-world");
        var result = hash.ComputeHash(bytes);

        Console.WriteLine(result.ToHexadecimalString());
    }
}

```
