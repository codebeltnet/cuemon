---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseFormatter`1
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseFormatter{TOptions}"/> to support content negotiation for exceptions.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.Configuration;
using Cuemon.Net.Http;
using Cuemon.AspNetCore.Diagnostics;

HttpExceptionDescriptorResponseFormatterExample.Demonstrate();

namespace Cuemon.AspNetCore.Diagnostics
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3cdf2c3b0adc0e028fa64fc7"><a href="~/run-actions/3cdf2c3b0adc0e028fa64fc7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3cdf2c3b0adc0e028fa64fc7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
