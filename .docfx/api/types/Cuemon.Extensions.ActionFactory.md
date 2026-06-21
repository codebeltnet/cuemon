---
uid: Cuemon.Extensions.ActionFactory
example:
- *content
---

The following example demonstrates how to create an `ActionFactory` from a callback with arguments and execute it. It shows both creating a factory with `Create` and invoking directly with `Invoke`.

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
