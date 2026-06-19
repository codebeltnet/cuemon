---
uid: Cuemon.AspNetCore.Http.PreconditionRequiredException
example:
- *content
---

The following example demonstrates how to use `PreconditionRequiredException` when a request requires conditional headers.

```csharp
using System;

namespace Cuemon.AspNetCore.Http;

public static class PreconditionRequiredExceptionExample
{
    public static void Demonstrate()
    {
        var hasConditionHeader = false;
        var exception = hasConditionHeader
            ? new PreconditionRequiredException()
            : new PreconditionRequiredException("Supply an If-Match header before retrying the update.");

        Console.WriteLine(exception.Message);
        Console.WriteLine(exception.StatusCode);
    }
}
```
