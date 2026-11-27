---
uid: Cuemon.AspNetCore.Mvc.Filters.Headers.UserAgentSentinelFilter
example:
- *content
---

The following example configures <xref cref="Cuemon.AspNetCore.Mvc.Filters.Headers.UserAgentSentinelFilter"/> to require and validate a known `User-Agent` header, then invokes `OnActionExecutionAsync` for both a missing header and an allowed header.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

var options = Options.Create(new UserAgentSentinelOptions
{
    RequireUserAgentHeader = true,
    ValidateUserAgentHeader = true
});
options.Value.AllowedUserAgents.Add("Cuemon-Agent");

var filter = new UserAgentSentinelFilter(options);

var missingContext = new ActionExecutingContext(
    new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
    new List<IFilterMetadata>(),
    new Dictionary<string, object>(),
    new object());

try
{
    await filter.OnActionExecutionAsync(missingContext, () =>
        Task.FromResult(new ActionExecutedContext(missingContext, new List<IFilterMetadata>(), new object())));
}
catch (UserAgentException ex)
{
    Console.WriteLine($"Missing User-Agent rejected: {(int)ex.StatusCode} ({ex.ReasonPhrase})");
}

var allowedHttpContext = new DefaultHttpContext();
allowedHttpContext.Request.Headers.UserAgent = "Cuemon-Agent";
var allowedContext = new ActionExecutingContext(
    new ActionContext(allowedHttpContext, new RouteData(), new ActionDescriptor()),
    new List<IFilterMetadata>(),
    new Dictionary<string, object>(),
    new object());
bool nextCalled = false;
await filter.OnActionExecutionAsync(allowedContext, () =>
{
    nextCalled = true;
    return Task.FromResult(new ActionExecutedContext(allowedContext, new List<IFilterMetadata>(), new object()));
});
Console.WriteLine($"Allowed User-Agent proceeded to action: {nextCalled}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="226149e7257c92931c2175d7"><a href="~/run-actions/226149e7257c92931c2175d7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/226149e7257c92931c2175d7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
