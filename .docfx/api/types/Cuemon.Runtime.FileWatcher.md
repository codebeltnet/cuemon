---
uid: Cuemon.Runtime.FileWatcher
example:
- *content
---

The following example shows how to start a file watcher, observe change notifications, and pause signaling again.

```csharp
using System;
using System.IO;
using System.Threading;
using Cuemon.Runtime;

namespace MyApp.Examples;

public static class FileWatcherExample
{
    public static void Demonstrate()
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, "health.txt");
        File.WriteAllText(filePath, "ready");

        using var watcher = new FileWatcher(filePath, readFile: true, options =>
        {
            options.DueTime = TimeSpan.Zero;
            options.Period = TimeSpan.FromSeconds(5);
            options.DueTimeOnChanged = TimeSpan.FromMilliseconds(250);
        });

        watcher.Changed += static (_, e) => Console.WriteLine($"{e.UtcLastModified:O} ({e.Delayed.TotalMilliseconds} ms)");
        watcher.StartMonitoring();

        Console.WriteLine(watcher.Path);
        Console.WriteLine(watcher.ReadFile);
        Console.WriteLine(watcher.UtcCreated.ToString("O"));

        watcher.ChangeSignaling(Timeout.InfiniteTimeSpan);
    }
}
```
