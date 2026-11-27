---
uid: Cuemon.Threading.TimerFactory
example:
- *content
---

The following example creates a non-capturing timer that fires every two seconds after an initial one-second delay. The callback prints "Timer ticked" each time it executes; the timer is disposed after five seconds.

```csharp
// Program.cs
using System;
using System.Threading;
using Cuemon.Threading;

using var timer = TimerFactory.CreateNonCapturingTimer(
    state => Console.WriteLine("Timer ticked"),
    null,
    TimeSpan.FromSeconds(1),
    TimeSpan.FromSeconds(2));

Console.WriteLine("Timer started (1s delay, 2s interval)");
Thread.Sleep(5000);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c8370b3d2ca0490027bb3a25"><a href="~/run-actions/c8370b3d2ca0490027bb3a25/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c8370b3d2ca0490027bb3a25/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
