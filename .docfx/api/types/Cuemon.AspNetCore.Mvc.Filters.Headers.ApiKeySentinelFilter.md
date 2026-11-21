---
uid: Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelFilter
example:
- *content
---

The following example creates <xref cref="Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelFilter"/> directly from configured options.

```csharp
// Program.cs
using System;
using System.Net;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Microsoft.Extensions.Options;

var options = Options.Create(new ApiKeySentinelOptions
{
    UseGenericResponse = true,
    GenericClientStatusCode = HttpStatusCode.NotFound,
    GenericClientMessage = "Resource not found."
});
options.Value.AllowedKeys.Add("Cuemon-Key");

var filter = new ApiKeySentinelFilter(options);

Console.WriteLine(filter.Options.HeaderName);
Console.WriteLine(filter.Options.AllowedKeys.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="55caa3a8e55dc0ce4742f425"><a href="~/run-actions/55caa3a8e55dc0ce4742f425/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/55caa3a8e55dc0ce4742f425/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
