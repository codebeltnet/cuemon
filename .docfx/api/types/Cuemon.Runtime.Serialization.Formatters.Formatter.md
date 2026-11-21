---
uid: Cuemon.Runtime.Serialization.Formatters.Formatter
example:
- *content
---

The following example shows how to resolve .NET types from qualified type-name strings using the `Formatter` class. It demonstrates both a direct resolution and a safe TryGetType call that avoids exceptions.

```csharp
// Program.cs
using System;
using Cuemon.Runtime.Serialization.Formatters;

var type = Formatter.GetType("System.DateTime, mscorlib");
Console.WriteLine($"Resolved type: {type}");

if (Formatter.TryGetType("Cuemon.GuidStringOptions, Cuemon.Core", out var optionsType))
{
    Console.WriteLine($"Found type: {optionsType}");
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ad6b693cb8151ff494a6b781"><a href="~/run-actions/ad6b693cb8151ff494a6b781/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ad6b693cb8151ff494a6b781/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
