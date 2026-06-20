---
uid: Cuemon.Security.FowlerNollVo32
example:
- *content
---

The following example demonstrates how to compute a 32-bit FNV-1a hash using the <see cref="FowlerNollVo32"/> class.

```csharp
using System;
using System.Text;
using Cuemon; // for Endianness
using Cuemon.Security; // for FowlerNollVo32

using Cuemon;
namespace MyApp.Examples;

public class FowlerNollVo32Example
{
    public void Demonstrate()
    {
        var fnv = new FowlerNollVo32();

        // Compute hash of a string
        byte[] data = Encoding.UTF8.GetBytes("hello");
        var hash = fnv.ComputeHash(data);

        Console.WriteLine(hash.ToHexadecimalString()); // F970D0C7 (big-endian default)
        Console.WriteLine(hash.ToBase64String());      // +XDQxw==
        Console.WriteLine(fnv.Bits);                   // 32

        // Configure for little-endian output
        var fnvLe = new FowlerNollVo32(o => o.ByteOrder = Endianness.LittleEndian);
        byte[] hashBytes = fnvLe.ComputeHash(data).GetBytes();
        Console.WriteLine(BitConverter.ToString(hashBytes)); // C7-D0-70-F9

        // Use FNV-1 variant instead of FNV-1a
        fnv.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1;
        hash = fnv.ComputeHash(data);
        Console.WriteLine(hash.ToHexadecimalString()); // E973FD3B

}
}

```
