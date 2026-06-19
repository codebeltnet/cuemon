---
uid: Cuemon.Eradicate
example:
- *content
---

The following example demonstrates how to use `Eradicate` to clean up byte arrays by removing trailing zero bytes or specific trailing byte sequences.

```csharp
using System;
using System.Text;

namespace Cuemon;

public class EradicateExample
{
    public void Demonstrate()
    {
        // Remove trailing zero bytes from a byte array
        byte[] dataWithZeros = { 1, 2, 3, 0, 0, 0 };
        byte[] cleaned = Eradicate.TrailingZeros(dataWithZeros);
        Console.WriteLine(BitConverter.ToString(cleaned)); // 01-02-03

        // Remove specific trailing byte sequence (e.g., CR/LF)
        byte[] dataWithCrLf = { 72, 101, 108, 108, 111, 13, 10, 13, 10 };
        byte[] stripped = Eradicate.TrailingBytes(dataWithCrLf, new byte[] { 13, 10 });
        Console.WriteLine(Encoding.UTF8.GetString(stripped)); // Hello
    }
}
```
