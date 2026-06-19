---
uid: Cuemon.Extensions.FuncFactory
example:
- *content
---

```csharp
using System;
using Cuemon;
using Cuemon.Extensions;

namespace Cuemon.Extensions;

public class FuncFactoryExample
{
    public void Demonstrate()
    {
        Func<int, int, string> formatter = (x, y) => $"{x} + {y} = {x + y}";

        var factory = FuncFactory.Create(formatter, 3, 4);
        string result = factory.ExecuteMethod();

        Console.WriteLine(result);

        int sum = FuncFactory.Invoke<MutableTuple<int, int>, int>(tuple => tuple.Arg1 + tuple.Arg2, MutableTupleFactory.CreateTwo(10, 20));
        Console.WriteLine(sum);
    }
}
```
