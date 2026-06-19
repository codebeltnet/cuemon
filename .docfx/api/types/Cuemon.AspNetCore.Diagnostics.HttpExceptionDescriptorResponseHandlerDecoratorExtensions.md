---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseHandlerDecoratorExtensions
example:
- *content
---

The following example demonstrates how to register additional HTTP exception descriptor response handlers using the decorator pattern.

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
