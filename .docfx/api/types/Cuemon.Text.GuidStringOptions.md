---
uid: Cuemon.Text.GuidStringOptions
example:
- *content
---

The following example demonstrates how to configure `GuidStringOptions` to restrict which GUID formats are accepted when parsing with `ParserFactory.FromGuid`.

```csharp
using System;
using Cuemon;
using Cuemon.Text;

namespace MyApp.Examples;

public class GuidStringOptionsExample
{
    public void Demonstrate()
    {
        // Direct instantiation of GuidStringOptions
        var options = new GuidStringOptions
        {
            Formats = GuidFormats.D
        };

        var parser = ParserFactory.FromGuid();
        var guidString = "{3f2504e0-4f89-41d3-9a0c-0305e82c3301}";

        var result = parser.Parse(guidString, o =>
        {
            o.Formats = GuidFormats.B;
        });

        Console.WriteLine(result);
        // outputs: 3f2504e0-4f89-41d3-9a0c-0305e82c3301

}
}

```
