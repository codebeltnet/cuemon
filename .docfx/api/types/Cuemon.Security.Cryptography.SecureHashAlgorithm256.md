---
uid: Cuemon.Security.Cryptography.SecureHashAlgorithm256
example:
- *content
---

The following example demonstrates how to compute a SHA-256 hash with <see cref="SecureHashAlgorithm256" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class SecureHashAlgorithm256Example
{
    public static void Demonstrate()
    {
        var algorithm = new SecureHashAlgorithm256();
        var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog"));
        var repeated = algorithm.ComputeHash(Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog"));

        Console.WriteLine(SecureHashAlgorithm256.BitSize);
        Console.WriteLine(result.GetBytes().Length);
        Console.WriteLine(result.Equals(repeated));
    }
}
```
