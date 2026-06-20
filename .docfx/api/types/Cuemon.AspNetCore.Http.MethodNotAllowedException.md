---
uid: Cuemon.AspNetCore.Http.MethodNotAllowedException
example:
- *content
---

The following example demonstrates how to use `MethodNotAllowedException` when a POST-only endpoint receives a GET request.

```csharp
using System;

namespace Cuemon.AspNetCore.Http;

public static class MethodNotAllowedExceptionExample
{
    public static void Demonstrate()
    {
        var allowedMethods = string.Join(", ", new[] { "POST", "PUT" });
        var exception = new MethodNotAllowedException($"Only {allowedMethods} are supported for /orders.");

        Console.WriteLine(exception.Message);
        Console.WriteLine(exception.StatusCode);
    }
}
```
