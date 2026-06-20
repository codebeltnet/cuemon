---
uid: Cuemon.Extensions.Collections.Generic.QueueExtensions
example:
- *content
---

The following example shows how to use `TryPeek` to safely inspect the front of a `Queue<string>` without removing the item. It demonstrates that successive calls return the same element.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Extensions.Collections.Generic;

namespace Cuemon.Extensions.Collections.Generic;

public class QueueExtensionsExample
{
    public void Demonstrate()
    {
        var queue = new Queue<string>();
        queue.Enqueue("first");
        queue.Enqueue("second");

        if (queue.TryPeek(out string result))
        {
            Console.WriteLine($"Peeked: {result}");
        }

        queue.TryPeek(out string same);
        Console.WriteLine(same);
    }
}
```
