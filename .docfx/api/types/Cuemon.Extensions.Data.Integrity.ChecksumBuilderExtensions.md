---
uid: Cuemon.Extensions.Data.Integrity.ChecksumBuilderExtensions
example:
- *content
---

The following example demonstrates how to use `ChecksumBuilderExtensions` to fluently combine typed values directly on a `ChecksumBuilder` instance.

```csharp
using System;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.Data.Integrity;
using Cuemon.Security;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {
        // Create a ChecksumBuilder and use extension methods directly
        var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());

        // Extension methods allow combining typed values without a Decorator wrapper
        builder.CombineWith(42);             // int overload
        builder.CombineWith(3.14);           // double overload
        builder.CombineWith("data");         // string overload

        Console.WriteLine($"Combined checksum: {builder}");

        // Works with all numeric types
        var checksum = new ChecksumBuilder(() => HashFactory.CreateFnv32())
            .CombineWith((short)1)
            .CombineWith(2u)
            .CombineWith(3L)
            .CombineWith(4.0f)
            .CombineWith(5ul);

        Console.WriteLine($"All types: {checksum}");

        // Combine with byte arrays
        var withBytes = new ChecksumBuilder(() => HashFactory.CreateFnv32())
            .CombineWith(new byte[] { 0x01, 0x02, 0x03 });

        Console.WriteLine($"With bytes: {withBytes}");
    }
}

```
