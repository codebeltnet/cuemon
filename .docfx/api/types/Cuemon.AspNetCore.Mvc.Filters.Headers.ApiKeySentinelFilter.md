---
uid: Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelFilter
example:
- *content
---

The following example configures <xref cref="Cuemon.AspNetCore.Mvc.Filters.Headers.ApiKeySentinelFilter"/> with `ApiKeySentinelOptions` (`UseGenericResponse = true`, `GenericClientStatusCode = HttpStatusCode.NotFound`, and an allowed key), then invokes `OnAuthorizationAsync` on `AuthorizationFilterContext` to observe a 404 result for missing keys and a null (authorized) result when the header matches.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

var options = Options.Create(new ApiKeySentinelOptions
{
    UseGenericResponse = true,
    GenericClientStatusCode = HttpStatusCode.NotFound,
    GenericClientMessage = "Resource not found."
});
options.Value.AllowedKeys.Add("Cuemon-Key");

var filter = new ApiKeySentinelFilter(options);

var deniedContext = new AuthorizationFilterContext(
    new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
    new List<IFilterMetadata>());
await filter.OnAuthorizationAsync(deniedContext);
var deniedStatus = (deniedContext.Result as IStatusCodeActionResult)?.StatusCode;
Console.WriteLine($"Missing key result: {deniedContext.Result?.GetType().Name}, status: {deniedStatus}");

var allowedHttpContext = new DefaultHttpContext();
allowedHttpContext.Request.Headers[filter.Options.HeaderName] = "Cuemon-Key";
var allowedContext = new AuthorizationFilterContext(
    new ActionContext(allowedHttpContext, new RouteData(), new ActionDescriptor()),
    new List<IFilterMetadata>());
await filter.OnAuthorizationAsync(allowedContext);
Console.WriteLine($"Allowed key result is null (authorized): {allowedContext.Result is null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="fb5b7847b9c770ab5ce8aa8b"><a href="~/run-actions/fb5b7847b9c770ab5ce8aa8b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/fb5b7847b9c770ab5ce8aa8b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
