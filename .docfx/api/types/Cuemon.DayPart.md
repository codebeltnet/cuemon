---
uid: Cuemon.DayPart
example:
- *content
---

The following example demonstrates how to use `DayPart` to determine the current part of the day and enumerate all built-in day parts.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon;

        var now = DateTime.Now.TimeOfDay;

        var current = DayPart.All.FirstOrDefault(dp =>
            now >= dp.Range.Start && now < dp.Range.End);

        Console.WriteLine($"Current time: {now:hh\\:mm}");
        Console.WriteLine($"Day part: {current?.Name ?? "Unknown"}");

        Console.WriteLine("All day parts:");
        foreach (var part in DayPart.All)
        {
            Console.WriteLine($"  {part}");

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0f19ce721851c5e0891be345"><a href="~/run-actions/0f19ce721851c5e0891be345/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0f19ce721851c5e0891be345/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
