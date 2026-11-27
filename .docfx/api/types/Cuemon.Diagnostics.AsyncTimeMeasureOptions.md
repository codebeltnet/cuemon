---
uid: Cuemon.Diagnostics.AsyncTimeMeasureOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="AsyncTimeMeasureOptions"/> for an asynchronous time measurement scenario.

```csharp
// Program.cs
using System;
using System.Threading;
using Cuemon.Diagnostics;
using Cuemon.Reflection;

var options = new AsyncTimeMeasureOptions
{
    TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(100),
    CancellationToken = CancellationToken.None,
    MethodDescriptor = () => MethodDescriptor.Create(typeof(AsyncTimeMeasureOptions).GetMethod(nameof(ToString))!),
    RuntimeParameters = new object[] { "warmup" }
};

Console.WriteLine(options.TimeMeasureCompletedThreshold);
Console.WriteLine(options.CancellationToken.CanBeCanceled);
Console.WriteLine(options.MethodDescriptor().MethodName);
Console.WriteLine(options.RuntimeParameters.Length);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3ec5f5ad48174f484467c21a"><a href="~/run-actions/3ec5f5ad48174f484467c21a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3ec5f5ad48174f484467c21a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
