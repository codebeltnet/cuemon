---
uid: Cuemon.Extensions.AspNetCore.Http.HttpExceptionDescriptorResponseFormatterExtensions
example:
- *content
---

The following example demonstrates how to project all exception descriptor handlers from a sequence of formatters into a single enumerable sequence.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http;

var formatters = new List<IHttpExceptionDescriptorResponseFormatter>
{
    new ExampleFormatter()
};

var handlers = formatters.SelectExceptionDescriptorHandlers().ToList();
Console.WriteLine(handlers.Count);
Console.WriteLine(handlers[0].ContentType.MediaType);

public sealed class ExampleFormatter : IHttpExceptionDescriptorResponseFormatter
{
    public ICollection<HttpExceptionDescriptorResponseHandler> ExceptionDescriptorHandlers { get; } =
        new List<HttpExceptionDescriptorResponseHandler>
        {
            new HttpExceptionDescriptorResponseHandler(
                new MediaTypeHeaderValue("application/json"),
                descriptor => new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(descriptor.Message)
                })
        };
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5ccff5f21ef3200384d0f12a"><a href="~/run-actions/5ccff5f21ef3200384d0f12a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5ccff5f21ef3200384d0f12a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
