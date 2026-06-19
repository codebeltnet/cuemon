---
uid: Cuemon.AspNetCore.Mvc.ForbiddenResult
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.ForbiddenResult"/> can return the default 403 status code or a different client-error code when you want to hide the existence of a protected resource.

```csharp
using System;
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MyApp.Examples;

public static class ForbiddenResultExample
{
    public static void Demonstrate()
    {
        var forbidden = new ForbiddenResult();
        var disguised = new ForbiddenResult(StatusCodes.Status404NotFound);

        Console.WriteLine(forbidden.StatusCode);
        Console.WriteLine(disguised.StatusCode);
    }
}
```
