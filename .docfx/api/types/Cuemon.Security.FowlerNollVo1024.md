---
uid: Cuemon.Security.FowlerNollVo1024
example:
- *content
---

The following example shows how to compute 1024-bit Fowler-Noll-Vo hashes for the same payload with different algorithms.

```csharp
using System;
using System.Text;
using Cuemon;
using Cuemon.Security;

namespace MyApp.Examples;

public static class FowlerNollVo1024Example
{
    public static void Demonstrate()
    {
        var hasher = new FowlerNollVo1024(options =>
        {
            options.Algorithm = FowlerNollVoAlgorithm.Fnv1a;
            options.ByteOrder = Endianness.BigEndian;
        });

        HashResult orderHash = hasher.ComputeHash(Encoding.UTF8.GetBytes("order-42"));

        Console.WriteLine(hasher.Bits);
        Console.WriteLine(orderHash.GetBytes().Length);
        Console.WriteLine(orderHash.ToHexadecimalString());

        var legacyHasher = new FowlerNollVo1024(options => options.Algorithm = FowlerNollVoAlgorithm.Fnv1);
        Console.WriteLine(legacyHasher.ComputeHash("order-42").ToHexadecimalString());
    }
}
```
