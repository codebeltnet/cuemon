---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseHandler
example:
- *content
---

The following example demonstrates how to use `HttpExceptionDescriptorResponseHandler` to write structured error responses.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Cuemon.AspNetCore.Diagnostics;

        var handler = HttpExceptionDescriptorResponseHandler.CreateDefaultFallbackHandler(FaultSensitivityDetails.None);
var exceptionDescriptor = new HttpExceptionDescriptor(new BadRequestException());

using var response = handler.ToHttpResponseMessage(exceptionDescriptor);
Console.WriteLine($"{(int)response.StatusCode} {handler.ContentType.MediaType}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="115d97ca6a9d32b192f7457b"><a href="~/run-actions/115d97ca6a9d32b192f7457b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/115d97ca6a9d32b192f7457b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
