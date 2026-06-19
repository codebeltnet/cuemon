---
uid: Cuemon.Security.FowlerNollVoOptions
example:
- *content
---

The following example demonstrates how to configure `FowlerNollVoOptions` with a specific algorithm and byte order, then use `FowlerNollVo32` to compute a hash.

```csharp
using System.Text;
using System;
using Cuemon.Security;

using Cuemon;
namespace MyApp.Examples;

public class FowlerNollVoOptionsExample
{
    public static void Main()
    {
        // Create a FowlerNollVoOptions instance to configure the hash algorithm
        var fnvOptions = new FowlerNollVoOptions();
        fnvOptions.Algorithm = FowlerNollVoAlgorithm.Fnv1a;
        fnvOptions.ByteOrder = Endianness.LittleEndian;

        // Apply configuration through the setup delegate
        var fnv32 = new FowlerNollVo32(o =>
        {
            o.Algorithm = fnvOptions.Algorithm;
            o.ByteOrder = fnvOptions.ByteOrder;
        });

        byte[] data = Encoding.UTF8.GetBytes("Hello, World!");
        HashResult hash = fnv32.ComputeHash(data);

        Console.WriteLine("FNV-1a 32-bit (little-endian): {0}", hash.ToHexadecimalString());

        // Output:
        // FNV-1a 32-bit (little-endian): 7b56c21a

}
}

```
