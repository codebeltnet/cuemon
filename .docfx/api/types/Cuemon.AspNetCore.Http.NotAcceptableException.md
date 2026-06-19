---
uid: Cuemon.AspNetCore.Http.NotAcceptableException
example:
- *content
---

The following example demonstrates how to configure an ASP.NET Core MVC filter that returns a `NotAcceptableException`.

```csharp
using System;

namespace Cuemon.AspNetCore.Http;

public static class NotAcceptableExceptionExample
{
    public static void Demonstrate()
    {
        var acceptHeader = "application/xml";
        var exception = new NotAcceptableException($"The endpoint cannot produce '{acceptHeader}'.");

        Console.WriteLine(exception.Message);
        Console.WriteLine(exception.StatusCode);
    }
}
```
