---
uid: Cuemon.Reflection.AssemblyContext
example:
- *content
---

The following example demonstrates how to filter and enumerate loaded assemblies using `AssemblyContext`. It retrieves all assemblies whose name starts with "Cuemon" and prints each assembly name.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.Reflection;

var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
{
    options.AssemblyFilter = a => a.FullName?.StartsWith("Cuemon") == true;
});

foreach (var assembly in assemblies)
{
    Console.WriteLine($"Assembly: {assembly.GetName().Name}");
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d289c39563c36beafdc42f19"><a href="~/run-actions/d289c39563c36beafdc42f19/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d289c39563c36beafdc42f19/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
