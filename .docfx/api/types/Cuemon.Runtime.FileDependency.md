---
uid: Cuemon.Runtime.FileDependency
example:
- *content
---

`FileDependency` defers `FileWatcher` creation until monitoring begins by accepting a `Lazy<FileWatcher>` factory. This example creates a temporary `settings.json` file, wraps a `FileWatcher` in a `Lazy<>` with a 500ms polling period, and passes it to `FileDependency` with `breakTieOnChanged: true`. Key steps include checking `lazyWatcher.IsValueCreated` before and after `StartAsync` to confirm deferred creation, subscribing to `DependencyChanged`, and inspecting `HasChanged` after signaling. Console output shows the watcher creation status, `BreakTieOnChanged` value, and the changed state.

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
