---
uid: Cuemon.Runtime.WatcherOptions
example:
- *content
---

The following example demonstrates how to configure a `WatcherOptions` for a file system watcher.

```csharp
// Program.cs
using System;
using Cuemon.Runtime;
using Examples;

Console.WriteLine(new WatcherConfigurationExample().ConfigureWatcher());

namespace Examples
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3c570e4be310f90ecd8e1ce1"><a href="~/run-actions/3c570e4be310f90ecd8e1ce1/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3c570e4be310f90ecd8e1ce1/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
