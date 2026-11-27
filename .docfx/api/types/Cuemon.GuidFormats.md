---
uid: Cuemon.GuidFormats
example:
- *content
---

The following example demonstrates how to use the <see cref="GuidFormats"/> flags enum to control which GUID formats are accepted during parsing.

```csharp
// Program.cs
using System;
using Cuemon; // for GuidFormats
using Cuemon.Text; // for ParserFactory, GuidStringOptions

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1951eb28808739078f8a7b09"><a href="~/run-actions/1951eb28808739078f8a7b09/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1951eb28808739078f8a7b09/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
