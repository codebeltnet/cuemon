---
uid: Cuemon.Extensions.Collections.Generic.CollectionExtensions
example:
- *content
---

The following example demonstrates how to add a range of values to a collection and iterate it through a partitioner.

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
