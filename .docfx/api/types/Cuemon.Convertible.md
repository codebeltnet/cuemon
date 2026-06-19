---
uid: Cuemon.Convertible
example:
- *content
---

```csharp
using System;
using Cuemon;

namespace MyApp.Conversion;

public class ConvertibleExample
{
    public void Demonstrate()
    {
        byte[] intBytes = Convertible.GetBytes(12345);
        Console.WriteLine(intBytes.Length); // 4
        Console.WriteLine(BitConverter.ToInt32(intBytes)); // 12345

        byte[] stringBytes = Convertible.GetBytes("Hello");
        Console.WriteLine(stringBytes.Length); // 5

        int restored = BitConverter.ToInt32(intBytes);
        Console.WriteLine(restored); // 12345
    }
}
```
