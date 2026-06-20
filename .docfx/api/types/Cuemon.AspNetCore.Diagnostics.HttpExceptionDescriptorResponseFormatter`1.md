---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseFormatter`1
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseFormatter{TOptions}"/> to support content negotiation for exceptions.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.Configuration;
using Cuemon.Net.Http;

namespace Cuemon.AspNetCore.Diagnostics;

public static class HttpExceptionDescriptorResponseFormatterExample
{
    public static void Demonstrate()
    {
        var formatter = new HttpExceptionDescriptorResponseFormatter<SampleFormatterOptions>(_ => { });

        formatter.Populate((exceptionDescriptor, mediaType) =>
            new StringContent($"{exceptionDescriptor.StatusCode}:{mediaType.MediaType}"));

        var exceptionDescriptor = new HttpExceptionDescriptor(
            new InvalidOperationException("boom"),
            418,
            "Teapot",
            "Short and stout");

        using var response = formatter.ExceptionDescriptorHandlers.First().ToHttpResponseMessage(exceptionDescriptor);
        Console.WriteLine(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
    }

    private sealed class SampleFormatterOptions : IContentNegotiation, IParameterObject
    {
        public IReadOnlyCollection<MediaTypeHeaderValue> SupportedMediaTypes { get; } =
            new[] { new MediaTypeHeaderValue("text/plain") };
    }
}
```
