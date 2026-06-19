---
uid: Cuemon.Diagnostics.TimeMeasureProfiler`1
example:
- *content
---

The following example demonstrates how to use <see cref="T:Cuemon.Diagnostics.TimeMeasure"/> to time an operation that returns a value and access the result through <see cref="Cuemon.Diagnostics.TimeMeasureProfiler{TResult}"/>.

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
