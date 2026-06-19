---
uid: Cuemon.AspNetCore.Mvc.GoneResult
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.GoneResult"/> can signal that a resource has been permanently removed.

```csharp
using System;
using Cuemon.AspNetCore.Mvc;

namespace MyApp.Examples;

public static class GoneResultExample
{
    public static void Demonstrate()
    {
        var result = new GoneResult();

        Console.WriteLine(result.StatusCode);
    }
}
```
