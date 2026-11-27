---
uid: Cuemon.AspNetCore.Http.BadRequestException
example:
- *content
---

The following example demonstrates how to use `BadRequestException` for model validation errors in an API controller.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

var missingFields = new[] { "email", "displayName" };
var exception = new BadRequestException(
    $"The JSON payload is missing required fields: {string.Join(", ", missingFields)}.",
    new FormatException("Unexpected end of JSON input."));

Console.WriteLine(exception.StatusCode);
Console.WriteLine(exception.Message);
Console.WriteLine(exception.InnerException?.Message);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="82c99822e33c2911937a0e68"><a href="~/run-actions/82c99822e33c2911937a0e68/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/82c99822e33c2911937a0e68/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
