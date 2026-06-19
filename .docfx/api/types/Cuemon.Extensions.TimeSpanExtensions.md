---
uid: Cuemon.Extensions.TimeSpanExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.TimeSpanExtensions> extension methods to round, floor, and ceiling <see cref="TimeSpan"/> values, and to retrieve high-resolution time units.

```csharp
using System;
using Cuemon;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class TimeSpanExtensionsExample
{
    public static void Demonstrate()
    {
        var hour = TimeSpan.FromHours(1);
        var duration = TimeSpan.FromMinutes(280);
        var shortDuration = TimeSpan.FromMinutes(45);

        Console.WriteLine(hour.GetTotalNanoseconds());
        Console.WriteLine(hour.GetTotalMicroseconds());
        Console.WriteLine(duration.Floor(1, TimeUnit.Hours));
        Console.WriteLine(duration.Ceiling(1, TimeUnit.Hours));
        Console.WriteLine(shortDuration.Round(TimeSpan.FromHours(1), VerticalDirection.Up));
        Console.WriteLine(shortDuration.Round(30, TimeUnit.Minutes, VerticalDirection.Down));
    }
}

```
