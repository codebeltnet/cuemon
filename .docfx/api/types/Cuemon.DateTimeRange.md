---
uid: Cuemon.DateTimeRange
example:
- *content
---

The following example demonstrates how to use `DateTimeRange` to represent and query a range between two `DateTime` values. It creates a range for January 2026, queries the start and end points, computes the duration, and tests equality against another range. Custom formatting and hash-code access are also shown. Each result is written to the console, confirming that date-range creation, comparison, and formatting behave as expected.

```csharp
// Program.cs
using System;
using System.Globalization;
using Cuemon;

// Create a date-time range representing January 2026
var start = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
var end = new DateTime(2026, 1, 31, 23, 59, 59, DateTimeKind.Utc);
var range = new DateTimeRange(start, end);

// Access start and end points
Console.WriteLine($"Start: {range.Start:O}");  // 2026-01-01T00:00:00.0000000Z
Console.WriteLine($"End:   {range.End:O}");    // 2026-01-31T23:59:59.0000000Z

// Duration between start and end
Console.WriteLine($"Duration: {range.Duration.Days} days"); // 30 days
Console.WriteLine($"Duration: {range.Duration}");          // 30.23:59:59

// Default ToString() formats duration and endpoints
Console.WriteLine(range.ToString()); // "A duration of 30.23:59:59 between 2026-01-01T00:00:00 and 2026-01-31T23:59:59."

// Custom format with invariant culture
Console.WriteLine(range.ToString("d", CultureInfo.InvariantCulture)); // "A duration of 30.23:59:59 between 01/01/2026 and 01/31/2026."

// Equality comparison via the inherited IEqualityComparer<Range<DateTime>>
var sameRange = new DateTimeRange(start, end);
Console.WriteLine(range.Equals(range, sameRange)); // True

var differentRange = new DateTimeRange(start, new DateTime(2026, 2, 1));
Console.WriteLine(range.Equals(range, differentRange)); // False

// Hash code based on start and end
var hash = range.GetHashCode(range);
Console.WriteLine($"Hash: {hash}");

// Practical use: measure a business period
var quarterStart = new DateTime(2026, 4, 1);
var quarterEnd = new DateTime(2026, 6, 30);
var q1 = new DateTimeRange(quarterStart, quarterEnd);
Console.WriteLine($"Q2 2026 lasts {q1.Duration.TotalDays} days."); // 90 days
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="864085ea1da2d353fed53d80"><a href="~/run-actions/864085ea1da2d353fed53d80/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/864085ea1da2d353fed53d80/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
