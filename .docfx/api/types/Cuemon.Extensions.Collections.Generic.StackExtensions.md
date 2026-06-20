---
uid: Cuemon.Extensions.Collections.Generic.StackExtensions
example:
- *content
---

The following example shows how to use `TryPop` to safely remove items from a `Stack<string>`. It demonstrates popping until the stack is empty, after which `TryPop` returns `false` and sets the result to `null`.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Extensions.Collections.Generic;

namespace Cuemon.Extensions.Collections.Generic;

public class StackExtensionsExample
{
    public void Demonstrate()
    {
        var stack = new Stack<string>();
        stack.Push("bottom");
        stack.Push("top");

        if (stack.TryPop(out string item))
        {
            Console.WriteLine($"Popped: {item}");
        }

        stack.TryPop(out string next);
        Console.WriteLine(next);

        stack.TryPop(out string empty);
        Console.WriteLine(empty == null);
    }
}
```
