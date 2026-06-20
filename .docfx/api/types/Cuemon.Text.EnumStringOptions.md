---
uid: Cuemon.Text.EnumStringOptions
example:
- *content
---

The following example demonstrates how to configure `EnumStringOptions` to control case sensitivity when parsing an enum from a string using `ParserFactory.FromEnum`.

```csharp
using System;
using Cuemon;
using Cuemon.Text;

namespace MyApp.Examples;

public class EnumStringOptionsExample
{
    public void Demonstrate()
    {
        // Direct instantiation of EnumStringOptions
        var options = new EnumStringOptions
        {
            IgnoreCase = true
        };

        var parser = ParserFactory.FromEnum();

        var result = (UriKind)parser.Parse("Relative", typeof(UriKind), o =>
        {
            o.IgnoreCase = true;
        });

        Console.WriteLine(result); // outputs: Relative

}
}

```
