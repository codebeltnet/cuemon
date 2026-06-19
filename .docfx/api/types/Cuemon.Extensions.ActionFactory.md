---
uid: Cuemon.Extensions.ActionFactory
example:
- *content
---

```csharp
using System;
using Cuemon;
using Cuemon.Extensions;

namespace Cuemon.Extensions;

public class ActionFactoryExample
{
    public void Demonstrate()
    {
        Action<string, int> callback = (name, count) =>
        {
            for (int i = 0; i < count; i++)
                Console.WriteLine(name);
        };

        var factory = ActionFactory.Create(callback, "Loop", 3);
        factory.ExecuteMethod();

        ActionFactory.Invoke(tuple => Console.WriteLine(tuple.Arg1), MutableTupleFactory.CreateOne("Direct invoke"));
    }
}
```
