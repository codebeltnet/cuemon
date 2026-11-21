---
uid: Cuemon.AspNetCore.Diagnostics.ServerTiming
example:
- *content
---

The following example demonstrates how to use `ServerTiming` to communicate performance metrics via the Server-Timing header.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;

        var timing = new ServerTiming();
timing.AddServerTiming("db", TimeSpan.FromMilliseconds(12), "SQL query");

var metric = timing.Metrics.First();
Console.WriteLine($"{metric.Name}:{metric.Duration}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e42820e117d0c9c78b0b235b"><a href="~/run-actions/e42820e117d0c9c78b0b235b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e42820e117d0c9c78b0b235b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
