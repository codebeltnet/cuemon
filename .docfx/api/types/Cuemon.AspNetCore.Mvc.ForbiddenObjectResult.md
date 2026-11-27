---
uid: Cuemon.AspNetCore.Mvc.ForbiddenObjectResult
example:
- *content
---

`ForbiddenObjectResult` is an `ObjectResult` subclass that returns a `403 Forbidden` HTTP response with an optional diagnostic payload. This example creates a first result with an anonymous object containing `error` and `requiredRole` fields, then inspects its `StatusCode` (403) and `Value`. A second result demonstrates overriding the status code to `404 Not Found` to obscure resource existence when security through obscurity is preferred. Console output confirms the status code values and the diagnostic payload content.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Mvc;

var example = new ForbiddenObjectResultExample();
Console.WriteLine(example.Demonstrate());
Console.WriteLine(example.DemonstrateWithCustomStatusCode());

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

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="86a0f98684f9cc2f4050535c"><a href="~/run-actions/86a0f98684f9cc2f4050535c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/86a0f98684f9cc2f4050535c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
