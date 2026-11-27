---
uid: Cuemon.Diagnostics.TimeMeasureProfiler`1
example:
- *content
---

`TimeMeasureProfiler<TResult>` extends `TimeMeasureProfiler` by providing typed access to the result of a timed function via `TimeMeasure.WithFunc`. This example wraps a `Thread.Sleep(100)` followed by returning `42`, then inspects the profiler's `Result` (`42`), `Elapsed` (~100ms), `IsRunning` (`False`), and `Member` properties. Key setup includes capturing the profiler and checking each property after execution. Console output shows the result value, the elapsed duration, the running state, the compiler-generated lambda member name, and the `ToString()` output showing the member name and elapsed time.

```csharp
// Program.cs
using System;
using System.Threading;
using Cuemon.Diagnostics;
using MyApp.Examples;

new TimeMeasureProfilerOfTExample().Demonstrate();

namespace MyApp.Examples
{
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
            Console.WriteLine($"Member: {profiler.Member}");        // MyApp.Examples.TimeMeasureProfilerOfTExample+<>c.<Demonstrate>b__0_0()
            Console.WriteLine(profiler.ToString());
            // Output: MyApp.Examples.TimeMeasureProfilerOfTExample+<>c.<Demonstrate>b__0_0() took ~00:00:00.100 to execute.

    }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0b479dc66a94c70f03627c54"><a href="~/run-actions/0b479dc66a94c70f03627c54/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0b479dc66a94c70f03627c54/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
