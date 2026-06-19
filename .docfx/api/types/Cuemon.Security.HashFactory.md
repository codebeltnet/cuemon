---
uid: Cuemon.Security.HashFactory
example:
- *content
---

```csharp
using System;
using System.Text;
using Cuemon.Security;

namespace Cuemon.Security;

public class HashFactoryExample
{
    public void Demonstrate()
    {
        var fnvHash = HashFactory.CreateFnv32();
        var input = "Hello World";
        var hash = fnvHash.ComputeHash(Encoding.UTF8.GetBytes(input));
        Console.WriteLine($"FNV-1a 32-bit: {BitConverter.ToString(hash.To<byte[]>(bytes => bytes)).Replace("-", "")}");

        var crcHash = HashFactory.CreateCrc32();
        hash = crcHash.ComputeHash(Encoding.UTF8.GetBytes(input));
        Console.WriteLine($"CRC-32: {BitConverter.ToString(hash.To<byte[]>(bytes => bytes)).Replace("-", "")}");

        var fnv256Hash = HashFactory.CreateFnv256();
        hash = fnv256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        Console.WriteLine($"FNV-1a 256-bit: {BitConverter.ToString(hash.To<byte[]>(bytes => bytes)).Replace("-", "")}");
    }
}
```
