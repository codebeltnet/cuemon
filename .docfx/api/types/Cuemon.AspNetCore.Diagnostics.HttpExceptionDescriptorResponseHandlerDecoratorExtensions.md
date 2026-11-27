---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseHandlerDecoratorExtensions
example:
- *content
---

`HttpExceptionDescriptorResponseHandlerDecoratorExtensions` provides extension methods on `Decorator.Enclose` for registering additional response handlers that control how HTTP exception descriptors are serialized and returned. This example creates an initial `HttpExceptionDescriptorResponseHandler` for `application/json` with a `500 Internal Server Error` status, wraps a `List<HttpExceptionDescriptorResponseHandler>` containing it, and calls `AddResponseHandler` with an options delegate specifying a custom content type, `ContentFactory`, and `StatusCodeFactory`. After execution, the list contains both handlers configured for different response scenarios in the error pipeline.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon;
using Cuemon.AspNetCore.Diagnostics;

var handler = new HttpExceptionDescriptorResponseHandler(
    new MediaTypeHeaderValue("application/json"),
    ed => new HttpResponseMessage(HttpStatusCode.InternalServerError)
    {
        Content = new StringContent(ed.Message)
    });
var list = new List<HttpExceptionDescriptorResponseHandler> { handler };

Decorator.Enclose(list).AddResponseHandler(o =>
{
    o.ContentType = new MediaTypeHeaderValue("application/json");
    o.ContentFactory = ed => new StringContent(ed.Message);
    o.StatusCodeFactory = ed => HttpStatusCode.InternalServerError;
});

Console.WriteLine($"Response handlers: {list.Count}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="912ff36e7843e049512c3941"><a href="~/run-actions/912ff36e7843e049512c3941/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/912ff36e7843e049512c3941/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
