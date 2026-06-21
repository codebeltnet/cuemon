---
uid: Cuemon.Diagnostics.TimeMeasureOptions
example:
- *content
---

The following example demonstrates how to configure <xref:Cuemon.Diagnostics.TimeMeasureOptions> for use with the <xref:Cuemon.Diagnostics.TimeMeasure> class.

```csharp
using System;
using System.Threading;
using Cuemon.Diagnostics;

namespace MyApp.Examples;

public class TimeMeasureOptionsExample
{
    public void Demonstrate()
    {
        // Configure options with a threshold
        var options = new TimeMeasureOptions
        {
            TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(100)
        };

        // Use with TimeMeasure to profile an action
        var profiler = TimeMeasure.WithAction(() =>
        {
            // Simulate work
            Thread.Sleep(50);
        }, setup: o =>
        {
            o.TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(100);
        });

        // The CompletedCallback will be invoked if elapsed time >= threshold
        Console.WriteLine($"Elapsed: {profiler.Elapsed}");

}
}

```
