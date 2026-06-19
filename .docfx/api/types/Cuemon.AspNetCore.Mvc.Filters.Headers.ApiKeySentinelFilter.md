---
uid: Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelFilter
example:
- *content
---

The following example creates <xref cref="Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelFilter"/> directly from configured options.

```csharp
using System;
using System.Net;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

public static class ApiKeySentinelFilterExample
{
    public static void Demonstrate()
    {
        var options = Options.Create(new ApiKeySentinelOptions
        {
            UseGenericResponse = true,
            GenericClientStatusCode = HttpStatusCode.NotFound,
            GenericClientMessage = "Resource not found."
        });
        options.Value.AllowedKeys.Add("Cuemon-Key");

        var filter = new ApiKeySentinelFilter(options);

        Console.WriteLine(filter.Options.HeaderName);
        Console.WriteLine(filter.Options.AllowedKeys.Count);
    }
}
```
