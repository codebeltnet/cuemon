---
uid: Cuemon.Extensions.AspNetCore.Authentication.AuthorizationResponseHandler
example:
- *content
---

The following example demonstrates how to construct `AuthorizationResponseHandler` with configured options and invoke `HandleAsync` for a forbidden authorization result. It registers `AuthorizationResponseHandlerOptions` and `ExceptionDescriptorOptions` with `FaultSensitivityDetails.All`, adds logging services, builds the service provider, and executes `HandleAsync` against a `DefaultHttpContext`, writing the resulting HTTP status code and response body to the console.

```csharp
// Program.cs
using System;
using System.IO;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();

services.AddAuthentication(BasicAuthorizationHeader.Scheme)
    .AddBasic(options =>
    {
        options.RequireSecureConnection = false;
        options.Authenticator = (_, _) => null;
    });
services.Configure<AuthorizationResponseHandlerOptions>(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.All;
});
services.Configure<ExceptionDescriptorOptions>(options =>
{
    options.SensitivityDetails = FaultSensitivityDetails.All;
});
services.AddLogging();

using var provider = services.BuildServiceProvider();
var logger = provider.GetRequiredService<ILogger<AuthorizationResponseHandler>>();
var options = provider.GetRequiredService<IOptions<AuthorizationResponseHandlerOptions>>();
var handler = new AuthorizationResponseHandler(logger, options);

var context = new DefaultHttpContext
{
    RequestServices = provider
};
context.Request.Headers.Accept = "text/plain";
context.Response.Body = new MemoryStream();

var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
var authorizeResult = PolicyAuthorizationResult.Forbid(AuthorizationFailure.ExplicitFail());

await handler.HandleAsync(_ => Task.CompletedTask, context, policy, authorizeResult);

context.Response.Body.Position = 0;
using var reader = new StreamReader(context.Response.Body);
Console.WriteLine($"Configured fault sensitivity: {handler.Options.SensitivityDetails}");
Console.WriteLine($"Status code: {context.Response.StatusCode}");
Console.WriteLine($"Response body: {await reader.ReadToEndAsync()}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3735678fc41f4532049c5db0"><a href="~/run-actions/3735678fc41f4532049c5db0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3735678fc41f4532049c5db0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
