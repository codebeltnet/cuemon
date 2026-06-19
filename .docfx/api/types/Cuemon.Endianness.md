---
uid: Cuemon.Endianness
example:
- *content
---

The following example demonstrates how to use `Endianness` to specify byte order when configuring `EndianOptions`.

```csharp
using Cuemon;
using System;

namespace MyApp.Examples;

public class EndiannessExample
{
    public void Demonstrate()
    {
        var options = new EndianOptions
        {
            ByteOrder = Endianness.BigEndian
        };

        Console.WriteLine(options.ByteOrder == Endianness.BigEndian);
        // outputs: True

}
}

```
