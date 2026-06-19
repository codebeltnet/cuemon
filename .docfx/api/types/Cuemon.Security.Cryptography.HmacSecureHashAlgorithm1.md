---
uid: Cuemon.Security.Cryptography.HmacSecureHashAlgorithm1
example:
- *content
---

The following example demonstrates how to compute an HMAC-SHA1 hash with <see cref="HmacSecureHashAlgorithm1" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class HmacSecureHashAlgorithm1Example
{
    public static void Demonstrate()
    {
        var algorithm = new HmacSecureHashAlgorithm1(CreateSecret(), null);
        var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Authenticate this message"));

        Console.WriteLine(result.GetBytes().Length);
        Console.WriteLine(result.ToBase64String());
    }

    private static byte[] CreateSecret() => Encoding.UTF8.GetBytes("docs-secret-sha1");
}
```
