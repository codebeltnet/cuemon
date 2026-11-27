---
uid: Cuemon.AspNetCore.Diagnostics.ServerTimingOptions
example:
- *content
---

The following example demonstrates how to configure `ServerTimingOptions`.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;

var options = new ServerTimingOptions
{
    TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(10)
};

Console.WriteLine($"Threshold: {options.TimeMeasureCompletedThreshold}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d4e360ed423b58ef93ba5af6"><a href="~/run-actions/d4e360ed423b58ef93ba5af6/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d4e360ed423b58ef93ba5af6/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
