---
uid: Cuemon.Runtime.WatcherEventArgs
example:
- *content
---

The following example shows the information a watcher passes along when a resource change is raised.

```csharp
// Program.cs
using System;
using Cuemon.Runtime;

var immediate = new WatcherEventArgs(DateTime.UtcNow);
var postponed = new WatcherEventArgs(DateTime.UtcNow.AddSeconds(-5), TimeSpan.FromMilliseconds(250));
var empty = WatcherEventArgs.Empty;

Console.WriteLine(immediate.UtcLastModified.Kind);
Console.WriteLine(postponed.Delayed.TotalMilliseconds);
Console.WriteLine(empty.UtcLastModified == DateTime.MinValue);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="fcf5ec8ead9633d1e9c1add3"><a href="~/run-actions/fcf5ec8ead9633d1e9c1add3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/fcf5ec8ead9633d1e9c1add3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
