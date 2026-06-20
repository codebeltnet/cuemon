---
uid: Cuemon.TimeUnit
example:
- *content
---

The following example demonstrates how to use the <see cref="TimeUnit"/> enum to convert a numeric value into a <see cref="TimeSpan"/>.

```csharp
using System;
using Cuemon; // for Decorator and ToTimeSpan

using Cuemon;
namespace MyApp.Examples;

public class TimeUnitExample
{
    public void Demonstrate()
    {
        double value = 2.5;

        TimeSpan days = Decorator.Enclose(value).ToTimeSpan(TimeUnit.Days);
        Console.WriteLine(days); // 2.12:00:00

        TimeSpan hours = Decorator.Enclose(value).ToTimeSpan(TimeUnit.Hours);
        Console.WriteLine(hours); // 02:30:00

        TimeSpan minutes = Decorator.Enclose(value).ToTimeSpan(TimeUnit.Minutes);
        Console.WriteLine(minutes); // 00:02:30

        TimeSpan seconds = Decorator.Enclose(value).ToTimeSpan(TimeUnit.Seconds);
        Console.WriteLine(seconds); // 00:00:02.5000000

        TimeSpan ticks = Decorator.Enclose(50000000.0).ToTimeSpan(TimeUnit.Ticks);
        Console.WriteLine(ticks); // 00:00:05

}
}

```
