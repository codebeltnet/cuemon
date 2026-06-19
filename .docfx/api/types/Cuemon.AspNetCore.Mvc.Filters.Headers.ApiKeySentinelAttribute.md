---
uid: Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelAttribute
example:
- *content
---

The following example applies <xref cref="Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelAttribute"/> to a controller and inspects the filter service type it resolves.

```csharp
using System;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Examples;

[ApiController]
[Route("api/[controller]")]
[ApiKeySentinel]
public sealed class SecureController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSecureData()
    {
        return Ok(new { Data = "Protected data" });
    }
}

public static class ApiKeySentinelAttributeExample
{
    public static void Demonstrate()
    {
        var attribute = new ApiKeySentinelAttribute();

        Console.WriteLine(attribute.ServiceType == typeof(ApiKeySentinelFilter));
        Console.WriteLine(attribute.IsReusable);
    }
}
```
