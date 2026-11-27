---
uid: Cuemon.Reflection.ManifestResourceMatch
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Reflection.ManifestResourceMatch"/> to specify how to locate embedded assembly resources.

```csharp
// Program.cs
using System;
using Cuemon.Reflection;

var match = ManifestResourceMatch.Extension;

switch (match)
{
    case ManifestResourceMatch.Name:
        Console.WriteLine("Match by exact resource name.");
        break;
    case ManifestResourceMatch.ContainsName:
        Console.WriteLine("Match by partial name match.");
        break;
    case ManifestResourceMatch.Extension:
        Console.WriteLine("Match by file extension (e.g., .json).");
        break;
    case ManifestResourceMatch.ContainsExtension:
        Console.WriteLine("Match by partial extension match.");
        break;

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="dea7db6ff31f2171b63e9110"><a href="~/run-actions/dea7db6ff31f2171b63e9110/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/dea7db6ff31f2171b63e9110/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
