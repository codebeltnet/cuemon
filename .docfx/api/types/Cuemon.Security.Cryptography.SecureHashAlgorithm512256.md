---
uid: Cuemon.Security.Cryptography.SecureHashAlgorithm512256
example:
- *content
---

The following example demonstrates how to compute a SHA-512/256 hash with <see cref="SecureHashAlgorithm512256" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class SecureHashAlgorithm512256Example
{
    public static void Demonstrate()
    {
        var algorithm = new SecureHashAlgorithm512256(null);
        var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Payload for SHA-512/256"));

        Console.WriteLine(SecureHashAlgorithm512256.BitSize);
        Console.WriteLine(result.GetBytes().Length);
    }
}
```
