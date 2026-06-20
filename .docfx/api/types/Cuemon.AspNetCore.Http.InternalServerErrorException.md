---
uid: Cuemon.AspNetCore.Http.InternalServerErrorException
example:
- *content
---

The following example demonstrates how to use `InternalServerErrorException` to represent an unexpected server-side failure in an exception handling middleware.

```csharp
using System;

namespace Cuemon.AspNetCore.Http;

public static class InternalServerErrorExceptionExample
{
    public static void Demonstrate()
    {
        var failure = new InvalidOperationException("The invoice pipeline failed to commit the transaction.");
        var exception = new InternalServerErrorException(failure);

        Console.WriteLine(exception.StatusCode);
        Console.WriteLine(exception.InnerException?.Message);
    }
}
```
