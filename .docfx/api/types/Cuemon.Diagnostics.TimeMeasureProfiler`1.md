---
uid: Cuemon.Diagnostics.TimeMeasureProfiler`1
example:
- *content
---

`TimeMeasureProfiler<TResult>` extends `TimeMeasureProfiler` by providing typed access to the result of a timed function via `TimeMeasure.WithFunc`. This example wraps a `Thread.Sleep(100)` followed by returning `42`, then inspects the profiler's `Result` (`42`), `Elapsed` (~100ms), `IsRunning` (`False`), and `Member` properties. Key setup includes capturing the profiler and checking each property after execution. Console output shows the result value, the elapsed duration, the running state, the member name, and the `ToString()` output like `<anonymous method> took 00:00:00.100 to execute.`.

```csharp
using System;
using System.Threading;
using Cuemon.Diagnostics;

namespace MyApp.Examples;

public class TimeMeasureProfilerOfTExample
{
    public void Demonstrate()
    {
        // TimeMeasure.WithFunc returns a TimeMeasureProfiler<int>
        TimeMeasureProfiler<int> profiler = TimeMeasure.WithFunc(() =>
        {
            Thread.Sleep(100);
            return 42;
        });

        Console.WriteLine($"Result: {profiler.Result}");       // 42
        Console.WriteLine($"Elapsed: {profiler.Elapsed}");     // ~00:00:00.100
        Console.WriteLine($"IsRunning: {profiler.IsRunning}");  // False
        Console.WriteLine($"Member: {profiler.Member}");        // <anonymous method>
        Console.WriteLine(profiler.ToString());
        // Output: <anonymous method> took 00:00:00.100 to execute.

}
}

```
