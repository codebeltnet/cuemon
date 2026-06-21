---
uid: Cuemon.Security.FowlerNollVo512
example:
- *content
---

The following example demonstrates how to compute 512-bit FNV-1a and FNV-1 hash values using FowlerNollVo512, with configurable algorithm variant and URL-safe Base64 output.

```csharp
using System;
using System.Text;
using Cuemon;
using Cuemon.Security;

namespace MyApp.Examples
{
    public class FowlerNollVo512Example
    {
        public void Demonstrate()
        {
            // Create a FNV-1a 512-bit hash (default algorithm).
            var fnv = new FowlerNollVo512();

            Console.WriteLine($"Bits:          {fnv.Bits}");       // 512
            Console.WriteLine($"Algorithm:     {fnv.Options.Algorithm}");   // Fnv1a
            Console.WriteLine($"Offset basis:  {fnv.OffsetBasis}");

            // Compute hash of binary data.
            var data = Encoding.UTF8.GetBytes("Cuemon FNV-512 example");
            var hash = fnv.ComputeHash(data);

            Console.WriteLine($"Hash (hex):    {hash.ToHexadecimalString()}");
            Console.WriteLine($"Hash (base64): {hash.ToBase64String()}");

            // Using FNV-1 algorithm.
            var fnv1 = new FowlerNollVo512(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            var hashFnv1 = fnv1.ComputeHash(data);
            Console.WriteLine($"FNV-1 hash:    {hashFnv1.ToHexadecimalString()}");

            // Raw bytes for further processing.
            var bytes = hash.GetBytes();
            Console.WriteLine($"Byte length:   {bytes.Length}");   // 64 (512 / 8)

            // URL-safe Base64 encoding.
            var urlSafe = hash.ToUrlEncodedBase64String();
            Console.WriteLine($"URL-safe b64:  {urlSafe}");

}}
}

```
