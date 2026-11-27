---
uid: Cuemon.AspNetCore.Http.UnauthorizedException
example:
- *content
---

The following example demonstrates how to use `UnauthorizedException` to indicate missing or invalid authentication.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

const string authenticationScheme = "Bearer";
var exception = new UnauthorizedException(
    $"The request is missing a valid {authenticationScheme} token.",
    new InvalidOperationException("Token validation failed."));

Console.WriteLine(exception.StatusCode);
Console.WriteLine(exception.Message);
Console.WriteLine(exception.InnerException?.Message);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3328fbd569b1713b52a4f681"><a href="~/run-actions/3328fbd569b1713b52a4f681/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3328fbd569b1713b52a4f681/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
