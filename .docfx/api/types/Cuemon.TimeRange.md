---
uid: Cuemon.TimeRange
example:
- *content
---

The following example demonstrates how to use <see cref="TimeRange"/> to represent a range between two <see cref="TimeSpan"/> values.

```csharp
// Program.cs
using System;
using Cuemon; // for TimeRange

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4b4ad5cd1d2d7bba0ef8c5c7"><a href="~/run-actions/4b4ad5cd1d2d7bba0ef8c5c7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4b4ad5cd1d2d7bba0ef8c5c7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
