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
        const string currentEtag = "\"v2\"";
        const string suppliedEtag = "\"v1\"";
        if (!string.Equals(suppliedEtag, currentEtag, StringComparison.Ordinal))
        {
            var exception = new PreconditionFailedException(
                $"The supplied If-Match value {suppliedEtag} does not match the current ETag {currentEtag}.",
                new InvalidOperationException("The resource changed after the client read it."));

            Console.WriteLine(exception.StatusCode);
            Console.WriteLine(exception.Message);
            Console.WriteLine(exception.InnerException?.Message);
        }
    }
}
```
