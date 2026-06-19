---
uid: Cuemon.Security.Cryptography.SecureHashAlgorithm384
example:
- *content
---

The following example demonstrates how to compute a SHA-384 hash with <see cref="SecureHashAlgorithm384" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class SecureHashAlgorithm384Example
{
    public static void Demonstrate()
    {
        var algorithm = new SecureHashAlgorithm384();
        var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Payload for SHA-384"));

        Console.WriteLine(SecureHashAlgorithm384.BitSize);
        Console.WriteLine(result.GetBytes().Length);
    }
}
```
