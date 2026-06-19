---
uid: Cuemon.Extensions.Collections.Generic.QueueExtensions
example:
- *content
---

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
