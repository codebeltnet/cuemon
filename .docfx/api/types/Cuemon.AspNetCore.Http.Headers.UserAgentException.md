---
uid: Cuemon.AspNetCore.Http.Headers.UserAgentException
example:
- *content
---

The following example demonstrates how a <xref cref="Cuemon.AspNetCore.Http.Headers.UserAgentException"/> is used to signal that a request's User-Agent header was rejected.

```csharp
using System;
using Microsoft.AspNetCore.Http;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class UserAgentExceptionExample
        {
            public static void Demonstrate()
            {
                var exception = new UserAgentException(StatusCodes.Status400BadRequest, "The User-Agent header is required.");
        Console.WriteLine($"{exception.StatusCode} {exception.ReasonPhrase}");
            }
        }
```
