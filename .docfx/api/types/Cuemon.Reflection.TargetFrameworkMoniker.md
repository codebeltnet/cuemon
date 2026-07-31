---
uid: Cuemon.Reflection.TargetFrameworkMoniker
example:
- *content
---

`TargetFrameworkMoniker` parses and resolves short target framework names such as `net10.0`, `net9.0`, `netstandard2.0`, and `net481` from framework names, assemblies, paths, or the current application context.

```csharp
using System;
using System.Reflection;
using Cuemon.Reflection;

namespace MyApp.Examples;

public class TargetFrameworkMonikerExample
{
    public void Demonstrate()
    {
        var parsed = TargetFrameworkMoniker.Parse(".NETCoreApp,Version=v10.0");
        var current = TargetFrameworkMoniker.ResolveCurrent();
        var library = TargetFrameworkMoniker.Resolve(typeof(TargetFrameworkMonikerExample).Assembly);
        var outputPath = TargetFrameworkMoniker.ResolveFromPath(AppContext.BaseDirectory);

        Console.WriteLine($"Parsed TFM: {parsed}");
        Console.WriteLine($"Current TFM: {current}");
        Console.WriteLine($"Example assembly TFM: {library}");
        Console.WriteLine($"Output path TFM: {outputPath}");

        if (TargetFrameworkMoniker.TryResolve(Assembly.GetExecutingAssembly(), out var executingAssemblyTfm))
        {
            Console.WriteLine($"Executing assembly TFM: {executingAssemblyTfm}");
        }
    }
}
```
