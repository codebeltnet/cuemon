---
uid: Cuemon.Security.Cryptography.HmacSecureHashAlgorithm512
example:
- *content
---

The following example demonstrates how different payloads produce different HMAC-SHA512 values with <see cref="HmacSecureHashAlgorithm512" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class HmacSecureHashAlgorithm512Example
{
    public static void Demonstrate()
    {
        var secret = Encoding.UTF8.GetBytes("docs-secret-sha512");
        var algorithm = new HmacSecureHashAlgorithm512(secret, null);
        var first = algorithm.ComputeHash(Encoding.UTF8.GetBytes("first"));
        var second = algorithm.ComputeHash(Encoding.UTF8.GetBytes("second"));

        Console.WriteLine(first.GetBytes().Length);
        Console.WriteLine(!first.Equals(second));
    }
}
```
