---
uid: Cuemon.MutableTuple`1
example:
- *content
---

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
