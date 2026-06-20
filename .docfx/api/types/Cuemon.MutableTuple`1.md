---
uid: Cuemon.MutableTuple`1
example:
- *content
---

The following example shows how to create a single-argument `MutableTuple<T>` and access its value. It demonstrates both reading and updating the `Arg1` property.

```csharp
using System;
using Cuemon;

namespace MyApp.Data;

public class MutableTupleExample
{
    public void Demonstrate()
    {
        var tuple = new MutableTuple<string>("example");
        Console.WriteLine(tuple.Arg1); // "example"

        tuple.Arg1 = "updated";
        Console.WriteLine(tuple.Arg1); // "updated"
    }
}
```
