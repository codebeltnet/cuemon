---
uid: Cuemon.Reflection.TargetFrameworkMoniker
example:
- *content
---

`TargetFrameworkMoniker` parses and resolves short target framework names such as `net10.0`, `net9.0`, `netstandard2.0`, and `net481` from framework names, assemblies, paths, or the current application context.

```csharp
// Program.cs
using System;
using System.Reflection;
using Cuemon.Reflection;

var parsed = TargetFrameworkMoniker.Parse(".NETCoreApp,Version=v10.0");
var current = TargetFrameworkMoniker.ResolveCurrent();
var library = TargetFrameworkMoniker.Resolve(typeof(TargetFrameworkMoniker).Assembly);
var outputPath = TargetFrameworkMoniker.ResolveFromPath(AppContext.BaseDirectory);

Console.WriteLine($"Parsed TFM: {parsed}");
Console.WriteLine($"Current TFM: {current}");
Console.WriteLine($"Example assembly TFM: {library}");
Console.WriteLine($"Output path TFM: {outputPath}");

if (TargetFrameworkMoniker.TryResolve(Assembly.GetExecutingAssembly(), out var executingAssemblyTfm))
{
    Console.WriteLine($"Executing assembly TFM: {executingAssemblyTfm}");
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="aff4a5a5f457fb4d51f3f330"><a href="~/run-actions/aff4a5a5f457fb4d51f3f330/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/aff4a5a5f457fb4d51f3f330/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
