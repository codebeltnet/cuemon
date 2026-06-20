---
uid: Cuemon.Runtime.WatcherEventArgs
example:
- *content
---

The following example shows the information a watcher passes along when a resource change is raised.

```csharp
using System;
using Cuemon.Runtime;

namespace MyApp.Examples;

public static class WatcherEventArgsExample
{
    public static void Demonstrate()
    {
        var immediate = new WatcherEventArgs(DateTime.UtcNow);
        var postponed = new WatcherEventArgs(DateTime.UtcNow.AddSeconds(-5), TimeSpan.FromMilliseconds(250));
        var empty = WatcherEventArgs.Empty;

        Console.WriteLine(immediate.UtcLastModified.Kind);
        Console.WriteLine(postponed.Delayed.TotalMilliseconds);
        Console.WriteLine(empty.UtcLastModified == DateTime.MinValue);
    }
}
```
