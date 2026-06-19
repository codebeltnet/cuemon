---
uid: Cuemon.AspNetCore.Mvc.ForbiddenObjectResult
example:
- *content
---

The following example demonstrates how to return a 403 Forbidden response with a diagnostic payload using `ForbiddenObjectResult`, optionally overriding the status code.

```csharp
using System;
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Mvc
{
    public class ForbiddenObjectResultExample
    {
        public IActionResult Demonstrate()
        {
            // Return 403 Forbidden with a diagnostic message
            var forbidden = new ForbiddenObjectResult(
                new { error = "Insufficient permissions", requiredRole = "admin" });

            Console.WriteLine($"Status code: {forbidden.StatusCode}");
            Console.WriteLine($"Value: {forbidden.Value}");

            return forbidden;
        }

        public IActionResult DemonstrateWithCustomStatusCode()
        {
            // Return 404 Not Found instead of 403 (to "hide" the resource existence)
            var hidden = new ForbiddenObjectResult(
                "Resource not found.",
                StatusCodes.Status404NotFound);

            Console.WriteLine($"Status code: {hidden.StatusCode}");

            return hidden;
        }
    }
}
```
