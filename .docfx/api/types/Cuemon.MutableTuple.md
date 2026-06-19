---
uid: Cuemon.MutableTuple
example:
- *content
---

The following example demonstrates how to create and use <xref:Cuemon.MutableTuple> and its generic variants to store and pass multiple values without creating a custom class.

```csharp
using System;
using Cuemon;

namespace Contoso.Workflow;

public sealed class MutableTupleExample
{
    public static void Run()
    {
        MutableTuple empty = new MutableTuple();
        var pair = new MutableTuple<string, int>("Alice", 30);

        pair.Arg2 = 31;

        object[] values = pair.ToArray("verified");
        var clone = (MutableTuple<string, int>)pair.Clone();

        Console.WriteLine($"Empty: {empty.IsEmpty}");
        Console.WriteLine($"{clone.Arg1}:{clone.Arg2}");
        Console.WriteLine($"Array length: {values.Length}");
    }
}
```
