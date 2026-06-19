---
uid: Cuemon.Reflection.MemberArgumentDecoratorExtensions
example:
- *content
---

The following example demonstrates how to reconstruct an exception from a recorded member argument stack using the <xref:Cuemon.Reflection.MemberArgumentDecoratorExtensions> class accessed through the <xref:Cuemon.Decorator> class.

```csharp
using System;
using System.Collections.Generic;
using Cuemon;
using Cuemon.Reflection;

namespace MyApp.Examples;

public class MemberArgumentDecoratorExtensionsExample
{
    public Exception ReconstructException()
    {
        // Simulate a recorded stack of member arguments representing an exception chain
        var stack = new Stack<IList<MemberArgument>>();

        var innerArgs = new List<MemberArgument>
        {
            new MemberArgument("type", typeof(InvalidOperationException)),
            new MemberArgument("message", "Inner operation failed.")
        };
        stack.Push(innerArgs);

        var outerArgs = new List<MemberArgument>
        {
            new MemberArgument("type", typeof(ArgumentException)),
            new MemberArgument("message", "Outer argument error."),
            new MemberArgument("paramName", "myParam"),
        };
        stack.Push(outerArgs);

        // Reconstruct the exception chain from the recorded arguments
        return Decorator.Enclose(stack).CreateException();

}
}

```
