---
uid: Cuemon.Runtime.FileDependency
example:
- *content
---

The following example shows how to defer file-watcher creation until a dependency starts monitoring.

```csharp
using System;
using System.IO;
using System.Threading.Tasks;
using Cuemon.Runtime;

namespace MyApp.Examples;

public static class FileDependencyExample
{
    public static async Task DemonstrateAsync()
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, "settings.json");
        File.WriteAllText(filePath, "{ }");

        var lazyWatcher = new Lazy<FileWatcher>(() => new FileWatcher(filePath, false, options =>
        {
            options.Period = TimeSpan.FromMilliseconds(500);
        }));

        var dependency = new FileDependency(lazyWatcher, breakTieOnChanged: true);
        dependency.DependencyChanged += static (_, e) => Console.WriteLine(e.UtcLastModified.ToString("O"));

        Console.WriteLine(lazyWatcher.IsValueCreated);
        Console.WriteLine(dependency.BreakTieOnChanged);

        await dependency.StartAsync();

        Console.WriteLine(lazyWatcher.IsValueCreated);
        Console.WriteLine(dependency.HasChanged);
    }
}
```
