---
uid: Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics.HttpFaultResolverExtensions
example:
- *content
---

The following example builds a resolver list the same way MVC fault handling does internally: map known exception types to HTTP-friendly descriptors, then ask a resolver to translate the thrown exception.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics;

var resolvers = new List<HttpFaultResolver>()
    .AddHttpFaultResolver<BadRequestException>(
        message: "The request payload could not be processed.",
        helpLink: new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400"))
    .AddHttpFaultResolver<TooManyRequestsException>(
        exception => new HttpExceptionDescriptor(exception),
        exception => exception is TooManyRequestsException);

var resolved = resolvers[1].TryResolveFault(new TooManyRequestsException(), out var descriptor);

Console.WriteLine($"{resolved}: {descriptor.StatusCode} {descriptor.Code}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="abb962aacf72012d3ddb5d77"><a href="~/run-actions/abb962aacf72012d3ddb5d77/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/abb962aacf72012d3ddb5d77/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
