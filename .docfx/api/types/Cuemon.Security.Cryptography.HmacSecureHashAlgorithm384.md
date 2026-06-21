---
uid: Cuemon.Security.Cryptography.HmacSecureHashAlgorithm384
example:
- *content
---

The following example demonstrates how to compute an HMAC-SHA384 hash with <see cref="HmacSecureHashAlgorithm384" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class HmacSecureHashAlgorithm384Example
{
    public static void Demonstrate()
    {
        var secret = Encoding.UTF8.GetBytes("docs-secret-sha384");
        var algorithm = new HmacSecureHashAlgorithm384(secret, null);
        var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Payload for SHA-384"));

        Console.WriteLine(result.GetBytes().Length);
        Console.WriteLine(result.ToBase64String());
    }
}
```
