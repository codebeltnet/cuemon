---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseHandlerOptions
example:
- *content
---

The following example demonstrates response handler options for HTTP exception descriptors.

```csharp
// Program.cs
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.AspNetCore.Diagnostics;

        var options = new HttpExceptionDescriptorResponseHandlerOptions
{
    ContentType = new MediaTypeHeaderValue("text/plain"),
    ContentFactory = exceptionDescriptor => new StringContent(exceptionDescriptor.Message),
    StatusCodeFactory = exceptionDescriptor => (HttpStatusCode)exceptionDescriptor.StatusCode
};

options.ValidateOptions();

Console.WriteLine(options.ContentType);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="72b9f0dfc2cca1c4ae936381"><a href="~/run-actions/72b9f0dfc2cca1c4ae936381/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/72b9f0dfc2cca1c4ae936381/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
