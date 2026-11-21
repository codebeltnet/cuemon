---
uid: Cuemon.Extensions.AspNetCore.Authentication.AuthorizationResponseHandler
example:
- *content
---

The following example demonstrates how to construct `AuthorizationResponseHandler` with configured options and the required logger dependency. It registers `AuthorizationResponseHandlerOptions` with `FaultSensitivityDetails.All`, adds logging services, and builds the service provider. The handler is then created from the resolved `ILogger<T>` and `IOptions<T>` instances, and its configured sensitivity is written to the console, confirming the dependency-injection wiring works correctly.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();

services.Configure<AuthorizationResponseHandlerOptions>(options =>
{
    options.SensitivityDetails = Cuemon.Diagnostics.FaultSensitivityDetails.All;
});

services.AddLogging();

using var provider = services.BuildServiceProvider();
var logger = provider.GetRequiredService<ILogger<AuthorizationResponseHandler>>();
var options = provider.GetRequiredService<IOptions<AuthorizationResponseHandlerOptions>>();
var handler = new AuthorizationResponseHandler(logger, options);

Console.WriteLine($"Configured fault sensitivity: {handler.Options.SensitivityDetails}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="191234b3da08096eb7c7766a"><a href="~/run-actions/191234b3da08096eb7c7766a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/191234b3da08096eb7c7766a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
