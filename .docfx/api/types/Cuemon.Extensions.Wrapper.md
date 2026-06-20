---
uid: Cuemon.Extensions.Wrapper
example:
- *content
---

The following example demonstrates how to wrap values using `Wrapper<T>` to access the inner instance, its type, and a parsed string representation. It shows wrapping both an integer and a string value.

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
