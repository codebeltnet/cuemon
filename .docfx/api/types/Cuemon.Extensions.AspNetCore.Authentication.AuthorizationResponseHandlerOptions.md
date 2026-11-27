---
uid: Cuemon.Extensions.AspNetCore.Authentication.AuthorizationResponseHandlerOptions
example:
- *content
---

The following example demonstrates how to configure `AuthorizationResponseHandlerOptions` to customize the behavior of the authorization response handler.

```csharp
// Program.cs
using System;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;

var options = new AuthorizationResponseHandlerOptions
{
    SensitivityDetails = FaultSensitivityDetails.All,
    FallbackResponseHandler = new AuthorizationMiddlewareResultHandler()
};

Console.WriteLine(options.SensitivityDetails);
Console.WriteLine(options.FallbackResponseHandler != null);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4f705d0afd3e55d53a10bfa5"><a href="~/run-actions/4f705d0afd3e55d53a10bfa5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4f705d0afd3e55d53a10bfa5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
