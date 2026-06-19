---
uid: Cuemon.Security.Cryptography.SHA512256
example:
- *content
---

The following example demonstrates how to use the low-level <see cref="SHA512256" /> implementation directly through <see cref="System.Security.Cryptography.HashAlgorithm" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class SHA512256Example
{
    public static void Demonstrate()
    {
        using var algorithm = new SHA512256();

        var digest = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Hello, world!"));
        algorithm.Initialize();
        var secondDigest = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Hello, world!"));

        Console.WriteLine(digest.Length);
        Console.WriteLine(digest.Length == secondDigest.Length);
    }
}
```
