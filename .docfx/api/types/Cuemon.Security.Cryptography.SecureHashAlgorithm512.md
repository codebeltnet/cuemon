---
uid: Cuemon.Security.Cryptography.SecureHashAlgorithm512
example:
- *content
---

The following example demonstrates how to compute a SHA-512 hash with <see cref="SecureHashAlgorithm512" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class SecureHashAlgorithm512Example
{
    public static void Demonstrate()
    {
        var algorithm = new SecureHashAlgorithm512();
        var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Payload for SHA-512"));

        Console.WriteLine(SecureHashAlgorithm512.BitSize);
        Console.WriteLine(result.ToBase64String());
    }
}
```
