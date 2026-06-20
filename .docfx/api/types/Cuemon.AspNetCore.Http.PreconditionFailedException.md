---
uid: Cuemon.AspNetCore.Http.PreconditionFailedException
example:
- *content
---

The following example demonstrates how to use `PreconditionFailedException` when a conditional request header check fails.

```csharp
using System;

namespace Cuemon.AspNetCore.Http;

public static class PreconditionFailedExceptionExample
{
    public static void Demonstrate()
    {
        var exception = new PreconditionFailedException(
            "The supplied If-Match value does not match the current ETag.",
            new InvalidOperationException("ETag mismatch."));

        Console.WriteLine(exception.StatusCode);
        Console.WriteLine(exception.InnerException?.Message);
    }
}
```
