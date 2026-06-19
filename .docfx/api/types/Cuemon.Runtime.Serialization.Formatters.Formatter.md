---
uid: Cuemon.Runtime.Serialization.Formatters.Formatter
example:
- *content
---

```csharp
using System;
using Cuemon.Runtime.Serialization.Formatters;

namespace Cuemon.Runtime.Serialization.Formatters;

public class FormatterExample
{
    public void Demonstrate()
    {
        var type = Formatter.GetType("System.DateTime, mscorlib");
        Console.WriteLine($"Resolved type: {type}");

        if (Formatter.TryGetType("Cuemon.GuidStringOptions, Cuemon.Core", out var optionsType))
        {
            Console.WriteLine($"Found type: {optionsType}");
        }
    }
}
```
