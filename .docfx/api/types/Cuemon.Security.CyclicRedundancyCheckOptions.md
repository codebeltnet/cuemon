---
uid: Cuemon.Security.CyclicRedundancyCheckOptions
example:
- *content
---

The following example shows how to reuse one `CyclicRedundancyCheckOptions` instance when configuring a checksum implementation.

```csharp
using System;
using System.Text;
using Cuemon;
using Cuemon.Security;

namespace MyApp.Examples;

public static class CyclicRedundancyCheckOptionsExample
{
    public static void Demonstrate()
    {
        var options = new CyclicRedundancyCheckOptions
        {
            ByteOrder = Endianness.LittleEndian,
            ReflectInput = true,
            ReflectOutput = true
        };

        var checksum = new CyclicRedundancyCheck32(setup: configured =>
        {
            configured.ByteOrder = options.ByteOrder;
            configured.ReflectInput = options.ReflectInput;
            configured.ReflectOutput = options.ReflectOutput;
        });

        Console.WriteLine(options.ByteOrder);
        Console.WriteLine(options.ReflectInput);
        Console.WriteLine(options.ReflectOutput);
        Console.WriteLine(checksum.ComputeHash(Encoding.ASCII.GetBytes("123456789")).ToHexadecimalString());
    }
}
```
