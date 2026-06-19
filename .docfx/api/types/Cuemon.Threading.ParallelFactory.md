---
uid: Cuemon.Threading.ParallelFactory
example:
- *content
---

```csharp
using System;
using Cuemon.Threading;

namespace Cuemon.Threading;

public class ParallelFactoryExample
{
    public void Demonstrate()
    {
        ParallelFactory.For(0, 5, i =>
        {
            Console.WriteLine($"Processing iteration {i}");
        });

        var items = new[] { "apple", "banana", "cherry" };
        ParallelFactory.ForEach(items, item =>
        {
            Console.WriteLine($"Processing {item}");
        });
    }
}
```
