---
uid: Cuemon.FuncFactory`2
example:
- *content
---

The following example demonstrates how to use <xref:Cuemon.FuncFactory`2> to encapsulate a function delegate and its arguments together for deferred execution.

```csharp
using System;
using Cuemon;

namespace Contoso.Formatting;

public sealed class FuncFactoryExample
{
    public static void Run()
    {
        var factory = new FuncFactory<MutableTuple<string, int>, string>(
            tuple => $"{tuple.Arg1}-{tuple.Arg2:D3}",
            new MutableTuple<string, int>("Item", 7));

        string result = factory.ExecuteMethod();
        var clone = (FuncFactory<MutableTuple<string, int>, string>)factory.Clone();

        Console.WriteLine(result);
        Console.WriteLine(clone.ExecuteMethod());
    }
}
```
