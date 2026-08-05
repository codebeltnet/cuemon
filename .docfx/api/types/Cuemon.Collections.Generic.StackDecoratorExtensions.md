---
uid: Cuemon.Collections.Generic.StackDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use `StackDecoratorExtensions.TryPop` to safely pop items from a `Stack` without throwing when it is empty. The decorator extension is present in the `netstandard2.0` asset, so the conditional branch calls it for .NET Framework consumers that select that asset; modern Cuemon.Core assets use the equivalent direct stack operation because the conditional extension type is not exposed there.

```csharp
using System;
using System.Collections.Generic;
using Cuemon;
#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER
using Cuemon.Collections.Generic;
#endif

namespace MyApp.Examples;

public class StackDecoratorExtensionsExample
{
    public static void Demonstrate()
    {
        var stack = new Stack<string>();
        stack.Push("first");
        stack.Push("second");
        stack.Push("third");

#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER
        var decorator = Decorator.Enclose(stack);
        while (decorator.TryPop(out string result))
        {
            Console.WriteLine("Popped: " + result);
        }
        Console.WriteLine("Stack is now empty");
#else
        while (stack.Count > 0)
        {
            Console.WriteLine("Popped: " + stack.Pop());
        }
        Console.WriteLine("Stack is now empty");
#endif
    }
}
```
