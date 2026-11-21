---
uid: Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelAttribute
example:
- *content
---

The following example applies <xref cref="Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelAttribute"/> to a controller and inspects the filter service type it resolves.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Microsoft.AspNetCore.Mvc;

var attribute = new ApiKeySentinelAttribute();

Console.WriteLine(attribute.ServiceType == typeof(ApiKeySentinelFilter));
Console.WriteLine(attribute.IsReusable);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f10ac0db264deb7bd03a1c1d"><a href="~/run-actions/f10ac0db264deb7bd03a1c1d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f10ac0db264deb7bd03a1c1d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
