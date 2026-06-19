---
uid: Cuemon.Diagnostics.TimeMeasureProfiler
example:
- *content
---

The following example demonstrates how <see cref="TimeMeasure"/> returns <see cref="TimeMeasureProfiler"/> instances for measured work.

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
