---
uid: Cuemon.Resilience.LatencyException
example:
- *content
---

The following example demonstrates how to wrap a timeout-related failure in a <see cref="LatencyException" />.

```csharp
// Program.cs
using System;
using Cuemon.Resilience;

var timeout = new TimeoutException("The database query timed out after 10 seconds.");
var exception = new LatencyException("Order processing exceeded the configured latency threshold.", timeout);

Console.WriteLine(exception.Message);
Console.WriteLine(exception.InnerException?.Message);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3cf6ccbbe5b170a6c71d8dcc"><a href="~/run-actions/3cf6ccbbe5b170a6c71d8dcc/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3cf6ccbbe5b170a6c71d8dcc/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
