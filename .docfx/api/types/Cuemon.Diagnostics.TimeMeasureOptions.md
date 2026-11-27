---
uid: Cuemon.Diagnostics.TimeMeasureOptions
example:
- *content
---

The following example demonstrates how to configure <xref:Cuemon.Diagnostics.TimeMeasureOptions> for use with the <xref:Cuemon.Diagnostics.TimeMeasure> class.

```csharp
// Program.cs
using System;
using System.Threading;
using Cuemon.Diagnostics;

// Configure options with a threshold
var options = new TimeMeasureOptions
{
    TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(100)
};

// Use with TimeMeasure to profile an action
var profiler = TimeMeasure.WithAction(() =>
{
    // Simulate work
    Thread.Sleep(50);
}, setup: o =>
{
    o.TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(100);
});

// The CompletedCallback will be invoked if elapsed time >= threshold
Console.WriteLine($"Elapsed: {profiler.Elapsed}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="dc303ae667b1fad5a852e79c"><a href="~/run-actions/dc303ae667b1fad5a852e79c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/dc303ae667b1fad5a852e79c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
