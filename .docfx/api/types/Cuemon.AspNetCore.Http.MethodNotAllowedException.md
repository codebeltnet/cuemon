---
uid: Cuemon.AspNetCore.Http.MethodNotAllowedException
example:
- *content
---

The following example demonstrates how to use `MethodNotAllowedException` when a POST-only endpoint receives a GET request.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

var allowedMethods = string.Join(", ", new[] { "POST", "PUT" });
var exception = new MethodNotAllowedException($"Only {allowedMethods} are supported for /orders.");

Console.WriteLine(exception.Message);
Console.WriteLine(exception.StatusCode);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d04b722e2fa86cb4be70e5ac"><a href="~/run-actions/d04b722e2fa86cb4be70e5ac/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d04b722e2fa86cb4be70e5ac/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
