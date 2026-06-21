---
uid: Cuemon.Security.Cryptography.HmacSecureHashAlgorithm256
example:
- *content
---

The following example demonstrates how to compute an HMAC-SHA256 hash with <see cref="HmacSecureHashAlgorithm256" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class HmacSecureHashAlgorithm256Example
{
    public static void Demonstrate()
    {
        var secret = Encoding.UTF8.GetBytes("unittest-secret");
        var payload = Encoding.UTF8.GetBytes("Authenticate this message");
        var algorithm = new HmacSecureHashAlgorithm256(secret, null);
        var result = algorithm.ComputeHash(payload);

        Console.WriteLine(result.GetBytes().Length);
        Console.WriteLine(result.ToHexadecimalString());
    }
}
```
