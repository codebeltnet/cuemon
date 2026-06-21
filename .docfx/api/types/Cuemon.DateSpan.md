---
uid: Cuemon.DateSpan
example:
- *content
---

The following example demonstrates how to calculate the time span between two dates using `DateSpan`. It shows constructing a span, accessing years/months/days, parsing ISO 8601 date strings, and creating a span that defaults the end to today.

```csharp
using System;
using Cuemon;

namespace MyApp.Time;

public class DateSpanExample
{
    public void Demonstrate()
    {
        // Create a DateSpan between two dates
        var start = new DateTime(2020, 1, 1);
        var end = new DateTime(2025, 6, 15);
        var span = new DateSpan(start, end);

        Console.WriteLine($"Years: {span.Years}");         // 5
        Console.WriteLine($"Months: {span.Months}");       // 65
        Console.WriteLine($"Days: {span.Days}");           // 1991
        Console.WriteLine($"Total days: {span.TotalDays:F1}"); // 1991.0

        // Parse from ISO 8601 strings
        var parsed = DateSpan.Parse("2020-01-01", "2025-06-15");
        Console.WriteLine(parsed.Years); // 5

        // Single DateSpan defaults the end to DateTime.Today
        var fromPast = new DateSpan(new DateTime(2023, 1, 1));
        Console.WriteLine($"Days since 2023-01-01: {fromPast.TotalDays:F0}");

}
}

```
