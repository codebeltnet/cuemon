---
uid: Cuemon.Extensions.Wrapper
example:
- *content
---

```csharp
using System;
using Cuemon;
using Cuemon.Extensions;

namespace Cuemon.Extensions;

public class WrapperExample
{
    public void Demonstrate()
    {
        var wrapped = new Wrapper<int>(42);
        Console.WriteLine(wrapped.Instance);
        Console.WriteLine(wrapped.InstanceType);

        string parsed = Wrapper.ParseInstance(wrapped);
        Console.WriteLine(parsed);

        int asInt = wrapped.InstanceAs<int>();
        Console.WriteLine(asInt);

        var wrappedString = new Wrapper<string>("Hello, World!");
        Console.WriteLine(Wrapper.ParseInstance(wrappedString));
    }
}
```
