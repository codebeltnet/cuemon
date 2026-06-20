---
uid: Cuemon.AspNetCore.Mvc.SeeOtherResult
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.SeeOtherResult"/> supports a 303 POST-redirect-GET response.

```csharp
using System;
using Cuemon.AspNetCore.Mvc;

namespace MyApp.Examples;

public static class SeeOtherResultExample
{
    public static void Demonstrate()
    {
        var result = new SeeOtherResult(new Uri("https://example.com/orders/42"));

        Console.WriteLine(result.StatusCode);
        Console.WriteLine(result.Location);
    }
}
```
