---
uid: Cuemon.AspNetCore.Mvc.TimeBasedObjectResultOptions`1
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Mvc.TimeBasedObjectResultOptions{T}"/> to provide timestamp providers for generating Last-Modified headers.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc;

var options = new TimeBasedObjectResultOptions<DateTime>
{
    TimestampProvider = value => value,
    ChangedTimestampProvider = value => value.AddHours(1)
};

// Validate that the required properties are configured
options.ValidateOptions();

var created = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
Console.WriteLine($"Created: {options.TimestampProvider(created)}");
Console.WriteLine($"Modified: {options.ChangedTimestampProvider(created)}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1baeacb0165e21bb6bfa3442"><a href="~/run-actions/1baeacb0165e21bb6bfa3442/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1baeacb0165e21bb6bfa3442/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
