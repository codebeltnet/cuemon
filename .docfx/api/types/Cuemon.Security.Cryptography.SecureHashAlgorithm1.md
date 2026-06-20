---
uid: Cuemon.Security.Cryptography.SecureHashAlgorithm1
example:
- *content
---

The following example demonstrates how to compute a SHA-1 hash with <see cref="SecureHashAlgorithm1" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class SecureHashAlgorithm1Example
{
    public static void Demonstrate()
    {
        var algorithm = new SecureHashAlgorithm1();
        var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("legacy-compatible digest"));

        Console.WriteLine(SecureHashAlgorithm1.BitSize);
        Console.WriteLine(result.GetBytes().Length);
    }
}
```
