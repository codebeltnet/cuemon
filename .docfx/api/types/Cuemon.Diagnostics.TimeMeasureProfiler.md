---
uid: Cuemon.Diagnostics.TimeMeasureProfiler
example:
- *content
---

`TimeMeasureProfiler` captures performance timing metrics for synchronous operations via the `TimeMeasure` API. This example calls `TimeMeasure.WithAction(() => Thread.Sleep(25))` to profile a 25ms sleep and `TimeMeasure.WithFunc(() => 42)` to profile a function returning a result. Key steps include checking the profiler's `Elapsed` time and `IsRunning` state, and accessing the `Result` of the function profiler. Console output confirms `Elapsed > TimeSpan.Zero` for both profilers, `IsRunning` is `False` after completion, and the function result is `42`.

```csharp
// Program.cs
using System;
using System.Threading;
using Cuemon.Diagnostics;

TimeMeasureProfiler profiler = TimeMeasure.WithAction(() => Thread.Sleep(25));
var measured = TimeMeasure.WithFunc(() => 42);

Console.WriteLine(profiler.Elapsed > TimeSpan.Zero);
Console.WriteLine(profiler.IsRunning);
Console.WriteLine(measured.Result);
Console.WriteLine(measured.Elapsed > TimeSpan.Zero);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f5a0f3e7f5c6c1ef7da01516"><a href="~/run-actions/f5a0f3e7f5c6c1ef7da01516/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f5a0f3e7f5c6c1ef7da01516/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
