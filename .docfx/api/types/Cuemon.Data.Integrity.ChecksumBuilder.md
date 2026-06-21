---
uid: Cuemon.Data.Integrity.ChecksumBuilder
example:
- *content
---

The following example demonstrates how to use `ChecksumBuilder` to compute and compare checksums for arbitrary data.

```csharp
using System;
using System.Text;
using Cuemon.Data.Integrity;
using Cuemon.Security;

namespace MyApp.Examples
{
    public sealed class ChecksumBuilderExample
    {
        public void Demonstrate()
        {
            var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
            Console.WriteLine($"Empty checksum: {builder.Checksum.ToHexadecimalString()}");

            builder.CombineWith(BitConverter.GetBytes(42L));
            builder.CombineWith(Encoding.UTF8.GetBytes("Hello, World!"));

            var comparison = new ChecksumBuilder(BitConverter.GetBytes(42L), () => HashFactory.CreateFnv128());
            comparison.CombineWith(Encoding.UTF8.GetBytes("Hello, World!"));

            Console.WriteLine($"Current checksum: {builder}");
            Console.WriteLine($"Checksums match: {builder.Equals(comparison)}");
            Console.WriteLine($"Hash code: {builder.GetHashCode()}");
        }
    }
}
```
