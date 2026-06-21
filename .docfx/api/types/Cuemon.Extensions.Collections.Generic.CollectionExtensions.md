---
uid: Cuemon.Extensions.Collections.Generic.CollectionExtensions
example:
- *content
---

`CollectionExtensions` provides extension methods for `ICollection<T>` including `AddRange` for bulk insertion and `ToPartitioner` for batched iteration. This example creates an empty `List<int>`, calls `AddRange(1, 2, 3, 4)` to insert four integers at once, then creates a partitioner with `ToPartitioner(2)` to split elements into batches of two. Console output confirms the element count (`4`) and the partition count (`2`), demonstrating batch processing of collection contents.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.Collections.Generic;

namespace MyApp.Examples;

public static class CollectionExtensionsExample
{
    public static void Demonstrate()
    {
        ICollection<int> values = new List<int>();
        values.AddRange(1, 2, 3, 4);

        var partitioner = values.ToPartitioner(2);

        Console.WriteLine(values.Count);
        Console.WriteLine(partitioner.Count());
    }
}
```
