---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseHandlerDecoratorExtensions
example:
- *content
---

`HttpExceptionDescriptorResponseHandlerDecoratorExtensions` provides extension methods on `Decorator.Enclose` for registering additional response handlers that control how HTTP exception descriptors are serialized and returned. This example creates an initial `HttpExceptionDescriptorResponseHandler` for `application/json` with a `500 Internal Server Error` status, wraps a `List<HttpExceptionDescriptorResponseHandler>` containing it, and calls `AddResponseHandler` with an options delegate specifying a custom content type, `ContentFactory`, and `StatusCodeFactory`. After execution, the list contains both handlers configured for different response scenarios in the error pipeline.

```csharp
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon;
using Cuemon.AspNetCore.Diagnostics;

namespace MyApp.Examples;

public class HttpExceptionDescriptorResponseHandlerDecoratorExtensionsExample
{
    public void Demonstrate()
    {
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
    }
}

```
