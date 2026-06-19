---
uid: Cuemon.AspNetCore.Http.Headers.ApiKeyException
example:
- *content
---

The following example demonstrates how a <xref cref="Cuemon.AspNetCore.Http.Headers.ApiKeyException"/> is used to signal that a request's API key header validation failed.

```csharp
using System;
using Microsoft.AspNetCore.Http;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class ApiKeyExceptionExample
        {
            public static void Demonstrate()
            {
                var exception = new ApiKeyException(StatusCodes.Status403Forbidden, "The API key was rejected.");
        Console.WriteLine($"{exception.StatusCode} {exception.ReasonPhrase}");
            }
        }
```
