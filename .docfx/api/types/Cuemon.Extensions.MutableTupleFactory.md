---
uid: Cuemon.Extensions.MutableTupleFactory
example:
- *content
---

```csharp
using System;
using Cuemon;
using Cuemon.Extensions;

namespace Cuemon.Extensions;

public class MutableTupleFactoryExample
{
    public void Demonstrate()
    {
        var zero = MutableTupleFactory.CreateZero();

        var one = MutableTupleFactory.CreateOne(42);
        Console.WriteLine(one.Arg1);

        var two = MutableTupleFactory.CreateTwo("Alice", 30);
        Console.WriteLine($"{two.Arg1} is {two.Arg2} years old");

        var three = MutableTupleFactory.CreateThree(1, 2, 3);

        var five = MutableTupleFactory.CreateFive('a', 'b', 'c', 'd', 'e');
    }
}
```
