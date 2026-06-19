---
uid: Cuemon.Diagnostics.AsyncTimeMeasureOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="AsyncTimeMeasureOptions"/> for an asynchronous time measurement scenario.

```csharp
using System;
using System.Threading;
using Cuemon.Diagnostics;
using Cuemon.Reflection;

namespace MyApp.Examples;

public static class AsyncTimeMeasureOptionsExample
{
    public static void Demonstrate()
    {
        var options = new AsyncTimeMeasureOptions
        {
            TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(100),
            CancellationToken = CancellationToken.None,
            MethodDescriptor = () => MethodDescriptor.Create(typeof(AsyncTimeMeasureOptionsExample).GetMethod(nameof(Demonstrate))!),
            RuntimeParameters = new object[] { "warmup" }
        };

        Console.WriteLine(options.TimeMeasureCompletedThreshold);
        Console.WriteLine(options.CancellationToken.CanBeCanceled);
        Console.WriteLine(options.MethodDescriptor().MethodName);
        Console.WriteLine(options.RuntimeParameters.Length);
    }
}

```
