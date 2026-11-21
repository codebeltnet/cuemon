---
uid: Cuemon.Extensions.TimeSpanExtensions
example:
- *content
---

`TimeSpanExtensions` provides extension methods for `TimeSpan` including high-resolution unit queries (`GetTotalNanoseconds`, `GetTotalMicroseconds`) and interval snapping (`Floor`, `Ceiling`, `Round`). This example creates `TimeSpan.FromHours(1)` and `TimeSpan.FromMinutes(280)`, then calls `GetTotalNanoseconds` and `GetTotalMicroseconds` on the hour, `Floor(1, TimeUnit.Hours)` and `Ceiling(1, TimeUnit.Hours)` on 280 minutes, and `Round` on 45 minutes with both up and down directions. Console output shows the nanosecond and microsecond values, the floored/ceiling hours, and the rounded minutes.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Extensions;

var hour = TimeSpan.FromHours(1);
var duration = TimeSpan.FromMinutes(280);
var shortDuration = TimeSpan.FromMinutes(45);

Console.WriteLine(hour.GetTotalNanoseconds());
Console.WriteLine(hour.GetTotalMicroseconds());
Console.WriteLine(duration.Floor(1, TimeUnit.Hours));
Console.WriteLine(duration.Ceiling(1, TimeUnit.Hours));
Console.WriteLine(shortDuration.Round(TimeSpan.FromHours(1), VerticalDirection.Up));
Console.WriteLine(shortDuration.Round(30, TimeUnit.Minutes, VerticalDirection.Down));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d2a9361be849962a624cb48d"><a href="~/run-actions/d2a9361be849962a624cb48d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d2a9361be849962a624cb48d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
