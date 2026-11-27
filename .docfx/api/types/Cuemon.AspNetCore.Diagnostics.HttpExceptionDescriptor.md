---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptor
example:
- *content
---

The following example demonstrates creating an HTTP exception descriptor with diagnostic evidence.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Diagnostics;

        var descriptor = new HttpExceptionDescriptor(new BadRequestException())
{
    Instance = new Uri("urn:request:42"),
    RequestId = "req-42",
    CorrelationId = "corr-42"
};

Console.WriteLine($"{descriptor.StatusCode} {descriptor.Message}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="641e3314607e0efbe1f4c1d7"><a href="~/run-actions/641e3314607e0efbe1f4c1d7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/641e3314607e0efbe1f4c1d7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
