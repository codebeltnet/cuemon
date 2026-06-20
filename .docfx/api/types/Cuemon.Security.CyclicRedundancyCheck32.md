---
uid: Cuemon.Security.CyclicRedundancyCheck32
example:
- *content
---

The following example shows how to compute a CRC-32 checksum with the common reflected configuration.

```csharp
using System;
using System.Text;
using Cuemon;
using Cuemon.Security;

namespace MyApp.Examples;

public static class CyclicRedundancyCheck32Example
{
    public static void Demonstrate()
    {
        var checksum = new CyclicRedundancyCheck32(setup: options =>
        {
            options.ByteOrder = Endianness.BigEndian;
            options.ReflectInput = true;
            options.ReflectOutput = true;
        });

        HashResult result = checksum.ComputeHash(Encoding.ASCII.GetBytes("123456789"));

        Console.WriteLine(result.ToHexadecimalString().ToLowerInvariant());
        Console.WriteLine(checksum.InitialValue);
        Console.WriteLine(checksum.FinalXor);

        var alternate = new CyclicRedundancyCheck32(polynomial: 0xEDB88320, initialValue: 0xFFFFFFFF, finalXor: 0xFFFFFFFF);
        Console.WriteLine(alternate.ComputeHash("Cuemon").ToBase64String());
    }
}
```
