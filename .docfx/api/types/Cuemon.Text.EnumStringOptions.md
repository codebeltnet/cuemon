---
uid: Cuemon.Text.EnumStringOptions
example:
- *content
---

The following example demonstrates how to configure `EnumStringOptions` to control case sensitivity when parsing an enum from a string using `ParserFactory.FromEnum`.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Text;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="216072929a2ccbe653e42ecc"><a href="~/run-actions/216072929a2ccbe653e42ecc/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/216072929a2ccbe653e42ecc/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
