---
uid: Cuemon.DayPart
example:
- *content
---

The following example demonstrates how to use `DayPart` to determine the current part of the day and enumerate all built-in day parts.

```csharp
using System;
using System.Linq;
using Cuemon;

namespace MyApp.Examples;

public class DayPartExample
{
    public void Demonstrate()
    {
        var now = DateTime.Now.TimeOfDay;

        var current = DayPart.All.FirstOrDefault(dp =>
            now >= dp.Range.Start && now < dp.Range.End);

        Console.WriteLine($"Current time: {now:hh\\:mm}");
        Console.WriteLine($"Day part: {current?.Name ?? "Unknown"}");

        Console.WriteLine("All day parts:");
        foreach (var part in DayPart.All)
        {
            Console.WriteLine($"  {part}");

}}
}

```
