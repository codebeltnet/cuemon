---
uid: Cuemon.AspNetCore.Http.Headers.UserAgentException
example:
- *content
---

The following example demonstrates how a <xref cref="Cuemon.AspNetCore.Http.Headers.UserAgentException"/> is used to signal that a request's User-Agent header was rejected.

```csharp
// Program.cs
using System;
using Microsoft.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;

        var exception = new UserAgentException(StatusCodes.Status400BadRequest, "The User-Agent header is required.");
Console.WriteLine($"{exception.StatusCode} {exception.ReasonPhrase}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f4961bbd14cf8c257ce320ca"><a href="~/run-actions/f4961bbd14cf8c257ce320ca/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f4961bbd14cf8c257ce320ca/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
