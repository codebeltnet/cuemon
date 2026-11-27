---
uid: Cuemon.AspNetCore.Mvc.SeeOtherResult
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.SeeOtherResult"/> supports a 303 POST-redirect-GET response.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc;

var result = new SeeOtherResult(new Uri("https://example.com/orders/42"));

Console.WriteLine(result.StatusCode);
Console.WriteLine(result.Location);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="cfa3512232ba117e79cecbd1"><a href="~/run-actions/cfa3512232ba117e79cecbd1/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/cfa3512232ba117e79cecbd1/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
