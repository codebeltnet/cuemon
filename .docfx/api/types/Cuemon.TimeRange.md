---
uid: Cuemon.TimeRange
example:
- *content
---

The following example demonstrates how to use <see cref="TimeRange"/> to represent a range between two <see cref="TimeSpan"/> values.

```csharp
using System;
using Cuemon; // for TimeRange

namespace MyApp.Examples;

public class TimeRangeExample
{
    public void Demonstrate()
    {
        // Define working hours (09:00 to 17:30)
        var workDay = new TimeRange(
            TimeSpan.FromHours(9),
            TimeSpan.FromHours(17.5));

        Console.WriteLine($"Start: {workDay.Start}");     // 09:00:00
        Console.WriteLine($"End: {workDay.End}");          // 17:30:00
        Console.WriteLine($"Duration: {workDay.Duration}"); // 08:30:00

        // Use inherited formatting
        Console.WriteLine(workDay.ToString("g", null));
        // Output: A duration of 00.08:30:00 between 09:00:00 and 17:30:00.

        // Create a short break range
        var lunchBreak = new TimeRange(
            TimeSpan.FromHours(12),
            TimeSpan.FromHours(13));
        Console.WriteLine(lunchBreak.Duration); // 01:00:00

}
}

```
