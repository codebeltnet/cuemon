---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseHandlerOptions
example:
- *content
---

The following example demonstrates response handler options for HTTP exception descriptors.

```csharp
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

        namespace Cuemon.AspNetCore.Diagnostics;

        public static class HttpExceptionDescriptorResponseHandlerOptionsExample
        {
            public static void Demonstrate()
            {
                var options = new HttpExceptionDescriptorResponseHandlerOptions
        {
            ContentType = new MediaTypeHeaderValue("text/plain"),
            ContentFactory = exceptionDescriptor => new StringContent(exceptionDescriptor.Message),
            StatusCodeFactory = exceptionDescriptor => (HttpStatusCode)exceptionDescriptor.StatusCode
        };

        options.ValidateOptions();
            }
        }
```
