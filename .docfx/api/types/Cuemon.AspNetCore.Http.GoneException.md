---
uid: Cuemon.AspNetCore.Http.GoneException
example:
- *content
---

The following example demonstrates how to return a `GoneException` from a deprecated API endpoint that has been removed.

```csharp
using System;

namespace Cuemon.AspNetCore.Http;

public static class GoneExceptionExample
{
    public static void Demonstrate()
    {
        var exception = CreateArchivedEndpointException("/v1/orders");

        Console.WriteLine(exception.Message);
        Console.WriteLine(exception.StatusCode);
    }

    private static GoneException CreateArchivedEndpointException(string route)
    {
        return new GoneException($"The resource at '{route}' has been retired.");
    }
}
```
