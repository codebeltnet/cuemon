---
uid: Cuemon.Resilience.TransientOperationOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="TransientOperationOptions" /> for retry attempts, transient-fault detection, and latency limits.

```csharp
// Program.cs
using System;
using System.Net.Http;
using Cuemon.Resilience;

var options = new TransientOperationOptions
{
    RetryAttempts = 3,
    RetryStrategy = attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
    DetectionStrategy = exception => exception is HttpRequestException,
    MaximumAllowedLatency = TimeSpan.FromSeconds(30)
};

options.ValidateOptions();

Console.WriteLine(options.RetryAttempts);
Console.WriteLine(options.EnableRecovery);
Console.WriteLine(options.MaximumAllowedLatency);
Console.WriteLine(options.DetectionStrategy(new HttpRequestException("Network timeout simulated.")));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="59df8043753b9d5b15f12cfd"><a href="~/run-actions/59df8043753b9d5b15f12cfd/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/59df8043753b9d5b15f12cfd/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
