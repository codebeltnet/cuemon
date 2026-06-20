---
uid: Cuemon.Diagnostics.TimeMeasure
example:
- *content
---

The following example demonstrates how to profile actions and functions using <see cref="TimeMeasure"/>. It profiles parameterless and parameterized actions via `WithAction`, profiles functions with return values via `WithFunc`, and configures a completion threshold. Each profiler's elapsed time and, where applicable, return value are written to the console, illustrating how to measure execution duration of synchronous delegates.

```csharp
using System;
using System.Threading;
using Cuemon.Diagnostics;

namespace MyApp.Examples;

public static class TimeMeasureExample
{
    public static void Demonstrate()
    {
        // Profile an action with no parameters
        TimeMeasureProfiler actionProfiler = TimeMeasure.WithAction(() =>
        {
            Thread.Sleep(50);
        });
        Console.WriteLine($"Action elapsed: {actionProfiler.Elapsed}");
        Console.WriteLine($"Member: {actionProfiler.Member}");

        // Profile an action with one parameter
        TimeMeasureProfiler paramProfiler = TimeMeasure.WithAction(
            (int ms) => Thread.Sleep(ms), 75);
        Console.WriteLine($"Param action elapsed: {paramProfiler.Elapsed}");

        // Profile a function that returns a value
        TimeMeasureProfiler<int> funcProfiler = TimeMeasure.WithFunc(() =>
        {
            Thread.Sleep(100);
            return 42;
        });
        Console.WriteLine($"Func result: {funcProfiler.Result}");
        Console.WriteLine($"Func elapsed: {funcProfiler.Elapsed}");

        // Profile a function with parameters
        TimeMeasureProfiler<string> greetProfiler = TimeMeasure.WithFunc(
            (string name) => $"Hello, {name}!", "World");
        Console.WriteLine($"Func result: {greetProfiler.Result}");

        // Configure options
        TimeMeasureProfiler configured = TimeMeasure.WithAction(
            () => Thread.Sleep(200),
            options => options.TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(50));
        Console.WriteLine($"Configured elapsed: {configured.Elapsed}");
    }
}
```
