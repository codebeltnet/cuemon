---
uid: Cuemon.AspNetCore.Http.UnauthorizedException
example:
- *content
---

The following example demonstrates how to use `UnauthorizedException` to indicate missing or invalid authentication.

```csharp
using System;

namespace Cuemon.AspNetCore.Http;

public static class UnauthorizedExceptionExample
{
    public static void Demonstrate()
    {
        const string authenticationScheme = "Bearer";
        var exception = new UnauthorizedException(
            $"The request is missing a valid {authenticationScheme} token.",
            new InvalidOperationException("Token validation failed."));

        Console.WriteLine(exception.StatusCode);
        Console.WriteLine(exception.Message);
        Console.WriteLine(exception.InnerException?.Message);
    }
}
```
