---
uid: Cuemon.Collections.Generic.StackDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use StackDecoratorExtensions.TryPop to safely pop items from a Stack without throwing exceptions when the stack is empty, by wrapping the Stack in an IDecorator and calling the TryPop extension method.

```csharp
using System;
using System.Collections.Generic;
using Cuemon;
using Cuemon.Collections.Generic;

namespace MyApp.Examples;

public class StackDecoratorExtensionsExample
{
    public static void Demonstrate()
    {
        var stack = new Stack<string>();
        stack.Push("first");
        stack.Push("second");
        stack.Push("third");

        var decorator = Decorator.Enclose(stack);

        if (StackDecoratorExtensions.TryPop(decorator, out string result1))
        {
            Console.WriteLine("Popped: " + result1);
        }

        if (StackDecoratorExtensions.TryPop(decorator, out string result2))
        {
            Console.WriteLine("Popped: " + result2);
        }

        if (StackDecoratorExtensions.TryPop(decorator, out string result3))
        {
            Console.WriteLine("Popped: " + result3);
        }

        if (!StackDecoratorExtensions.TryPop(decorator, out string _))
        {
            Console.WriteLine("Stack is now empty");
        }
    }
}
```
