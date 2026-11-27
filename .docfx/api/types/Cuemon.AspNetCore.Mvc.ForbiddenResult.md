---
uid: Cuemon.AspNetCore.Mvc.ForbiddenResult
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.ForbiddenResult"/> can return the default 403 status code or a different client-error code when you want to hide the existence of a protected resource.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

var forbidden = new ForbiddenResult();
var disguised = new ForbiddenResult(StatusCodes.Status404NotFound);

Console.WriteLine(forbidden.StatusCode);
Console.WriteLine(disguised.StatusCode);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="44457624f72a018856da7cdb"><a href="~/run-actions/44457624f72a018856da7cdb/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/44457624f72a018856da7cdb/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
