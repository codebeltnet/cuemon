---
uid: Cuemon.Threading.ParallelFactory
example:
- *content
---

The following example demonstrates basic parallel loops using `ParallelFactory.For` and `ParallelFactory.ForEach`. Each iteration prints its index or item to the console, illustrating how the factory distributes work across threads.

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
