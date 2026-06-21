---
uid: Cuemon.Runtime.WatcherOptions
example:
- *content
---

The following example demonstrates how to configure a `WatcherOptions` for a file system watcher.

```csharp
using System;
using Cuemon.Runtime;

namespace Examples;

public class WatcherConfigurationExample
{
    public WatcherOptions ConfigureWatcher()
    {
        return new WatcherOptions
        {
            DueTime = TimeSpan.FromSeconds(5),
            DueTimeOnChanged = TimeSpan.FromSeconds(2),
            Period = TimeSpan.FromMinutes(1)
        };

}
}

```
