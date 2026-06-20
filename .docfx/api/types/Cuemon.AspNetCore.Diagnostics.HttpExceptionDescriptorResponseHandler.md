---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorResponseHandler
example:
- *content
---

The following example demonstrates how to use `HttpExceptionDescriptorResponseHandler` to write structured error responses.

```csharp
using System;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;

        namespace Cuemon.AspNetCore.Diagnostics;

        public static class HttpExceptionDescriptorResponseHandlerExample
        {
            public static void Demonstrate()
            {
                var handler = HttpExceptionDescriptorResponseHandler.CreateDefaultFallbackHandler(FaultSensitivityDetails.None);
        var exceptionDescriptor = new HttpExceptionDescriptor(new BadRequestException());

        using var response = handler.ToHttpResponseMessage(exceptionDescriptor);
        Console.WriteLine($"{(int)response.StatusCode} {handler.ContentType.MediaType}");
            }
        }
```
