---
uid: Cuemon.EndianOptions
example:
- *content
---

The following example demonstrates how to configure `EndianOptions` to explicitly set the byte order for binary data operations.

```csharp
using System;
using Cuemon;

namespace MyApp.Examples;

public class EndianOptionsExample
{
    public void Demonstrate()
    {
        var options = new EndianOptions
        {
            ByteOrder = Endianness.BigEndian
        };

        var byteOrder = options.ByteOrder;
        var isSystemLittleEndian = BitConverter.IsLittleEndian;

        Console.WriteLine($"Configured byte order: {byteOrder}");
        Console.WriteLine($"System is little-endian: {isSystemLittleEndian}");

}
}

```
