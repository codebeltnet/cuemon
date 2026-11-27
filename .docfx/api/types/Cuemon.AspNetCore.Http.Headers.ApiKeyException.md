---
uid: Cuemon.AspNetCore.Http.Headers.ApiKeyException
example:
- *content
---

The following example demonstrates how a <xref cref="Cuemon.AspNetCore.Http.Headers.ApiKeyException"/> is used to signal that a request's API key header validation failed.

```csharp
// Program.cs
using System;
using Microsoft.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;

        var exception = new ApiKeyException(StatusCodes.Status403Forbidden, "The API key was rejected.");
Console.WriteLine($"{exception.StatusCode} {exception.ReasonPhrase}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5bfeeb175e4542b6d7995ded"><a href="~/run-actions/5bfeeb175e4542b6d7995ded/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5bfeeb175e4542b6d7995ded/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
