---
uid: Cuemon.Diagnostics.TimeMeasureProfiler
example:
- *content
---

`TimeMeasureProfiler` captures performance timing metrics for synchronous operations via the `TimeMeasure` API. This example calls `TimeMeasure.WithAction(() => Thread.Sleep(25))` to profile a 25ms sleep and `TimeMeasure.WithFunc(() => 42)` to profile a function returning a result. Key steps include checking the profiler's `Elapsed` time and `IsRunning` state, and accessing the `Result` of the function profiler. Console output confirms `Elapsed > TimeSpan.Zero` for both profilers, `IsRunning` is `False` after completion, and the function result is `42`.

```csharp
using System;
using System.Threading;
using Cuemon.Diagnostics;

namespace MyApp.Examples;

public static class TimeMeasureProfilerExample
{
    public static void Demonstrate()
    {
        TimeMeasureProfiler profiler = TimeMeasure.WithAction(() => Thread.Sleep(25));
        var measured = TimeMeasure.WithFunc(() => 42);

        Console.WriteLine(profiler.Elapsed > TimeSpan.Zero);
        Console.WriteLine(profiler.IsRunning);
        Console.WriteLine(measured.Result);
        Console.WriteLine(measured.Elapsed > TimeSpan.Zero);
    }
}

```
