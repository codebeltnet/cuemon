---
uid: Cuemon.Reflection.ActivatorFactory
example:
- *content
---

```csharp
using System;
using System.Text;
using Cuemon.Reflection;

namespace Cuemon.Reflection;

public class ActivatorFactoryExample
{
    public void Demonstrate()
    {
        var sb = ActivatorFactory.CreateInstance<StringBuilder>();
        sb.Append("Hello from activator");
        Console.WriteLine(sb.ToString());

        var dt = ActivatorFactory.CreateInstance<int, int, int, DateTime>(2025, 12, 1);
        Console.WriteLine($"Created: {dt:yyyy-MM-dd}");
    }
}
```
