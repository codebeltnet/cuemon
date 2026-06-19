---
uid: Cuemon.GuidFormats
example:
- *content
---

The following example demonstrates how to use the <see cref="GuidFormats"/> flags enum to control which GUID formats are accepted during parsing.

```csharp
using System;
using Cuemon; // for GuidFormats
using Cuemon.Text; // for ParserFactory, GuidStringOptions

namespace MyApp.Examples;

public class GuidFormatsExample
{
    public void Demonstrate()
    {
        var parser = ParserFactory.FromGuid();

        // Accept only digit (D) and brace (B) formats
        Guid g1 = parser.Parse("12345678-1234-1234-1234-123456789abc",
            o => o.Formats = GuidFormats.D | GuidFormats.B);
        Console.WriteLine(g1); // 12345678-1234-1234-1234-123456789abc

        Guid g2 = parser.Parse("{12345678-1234-1234-1234-123456789abc}",
            o => o.Formats = GuidFormats.D | GuidFormats.B);
        Console.WriteLine(g2);

        // TryParse with number format (N) - using Hyphens, so it would fail
        bool ok = parser.TryParse("12345678123412341234123456789abc", out Guid g3,
            o => o.Formats = GuidFormats.N);
        Console.WriteLine(ok); // True - only N is selected and input has no hyphens

        // Fail: brace format but only D selected
        ok = parser.TryParse("{12345678-1234-1234-1234-123456789abc}", out Guid g4,
            o => o.Formats = GuidFormats.D);
        Console.WriteLine(ok); // False

}
}

```
