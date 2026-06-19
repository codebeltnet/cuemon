---
uid: Cuemon.Security.FowlerNollVo64
example:
- *content
---

The following example demonstrates how to compute a 64-bit Fowler-Noll-Vo (FNV) hash using the <xref:Cuemon.Security.FowlerNollVo64> class.

```csharp
using System;
using System.Text;
using Cuemon.Security;

using Cuemon;
namespace MyApp.Examples;

public class FowlerNollVo64Example
{
    public void Demonstrate()
    {
        // Create FNV-1a 64-bit hash instance (default algorithm)
        var fnv = new FowlerNollVo64();

        // Compute hash from a string
        byte[] data = Encoding.UTF8.GetBytes("hello");
        HashResult hash = fnv.ComputeHash(data);

        // Display the hash in hexadecimal format
        Console.WriteLine(hash.ToHexadecimalString());

        // Switch to FNV-1 algorithm
        fnv.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1;
        HashResult hashFnv1 = fnv.ComputeHash(data);
        Console.WriteLine(hashFnv1.ToHexadecimalString());

        // Change byte order to little endian
        fnv.Options.ByteOrder = Endianness.LittleEndian;
        HashResult hashLe = fnv.ComputeHash(data);
        Console.WriteLine(hashLe.ToHexadecimalString());

}
}

```
