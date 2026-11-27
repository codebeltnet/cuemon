---
uid: Cuemon.AspNetCore.Http.ForbiddenException
example:
- *content
---

The following example demonstrates how to use `ForbiddenException` to reject unauthorized access in an authorization filter.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

try
{
    var userRole = "guest";
    if (userRole != "admin")
    {
        throw new ForbiddenException("Only administrators can perform this action.");
    }
}
catch (ForbiddenException ex)
{
    Console.WriteLine(ex.StatusCode); // 403
    Console.WriteLine(ex.Message);    // Only administrators can perform this action.
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="fa5fe03dbb66e3eb809adcde"><a href="~/run-actions/fa5fe03dbb66e3eb809adcde/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/fa5fe03dbb66e3eb809adcde/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
