---
uid: Cuemon.AspNetCore.Http.BadRequestException
example:
- *content
---

The following example demonstrates how to use `BadRequestException` for model validation errors in an API controller.

```csharp
using System;

namespace Cuemon.AspNetCore.Http;

public static class BadRequestExceptionExample
{
    public static void Demonstrate()
    {
        var exception = new BadRequestException(
            "The JSON payload is missing the required 'email' field.",
            new FormatException("Unexpected end of JSON input."));

        Console.WriteLine(exception.StatusCode);
        Console.WriteLine(exception.InnerException?.GetType().Name);
    }
}
```
