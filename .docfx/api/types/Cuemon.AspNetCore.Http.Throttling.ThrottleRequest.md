---
uid: Cuemon.AspNetCore.Http.Throttling.ThrottleRequest
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Http.Throttling.ThrottleRequest"/> to track HTTP request usage and quota in a throttling scenario.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http.Throttling;

        var request = new ThrottleRequest(new ThrottleQuota(10, TimeSpan.FromMinutes(1)));
request.IncrementTotal();

Console.WriteLine(request.Total);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3497bf7eb6ead0a41a1c7f1f"><a href="~/run-actions/3497bf7eb6ead0a41a1c7f1f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3497bf7eb6ead0a41a1c7f1f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
