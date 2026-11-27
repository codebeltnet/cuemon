---
uid: Cuemon.Extensions.AspNetCore.Authentication.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register the in-memory digest nonce tracker and the authorization response handler services. It inserts and reads a nonce to verify the tracker registration, then reports the configured fault-sensitivity option used by the authorization response handler.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddLogging();
services.AddInMemoryDigestAuthenticationNonceTracker();
services.AddAuthorizationResponseHandler(o =>
{
    o.SensitivityDetails = FaultSensitivityDetails.All;
});

using var provider = services.BuildServiceProvider();

var tracker = provider.GetRequiredService<INonceTracker>();
var added = tracker.TryAddEntry("docs-nonce", 1);
var found = tracker.TryGetEntry("docs-nonce", out var entry);
var responseHandler = provider.GetRequiredService<AuthorizationResponseHandler>();

Console.WriteLine($"Nonce added: {added}; found: {found}; count: {entry?.Count}");
Console.WriteLine($"Configured fault sensitivity: {responseHandler.Options.SensitivityDetails}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0cfaac0e4511ead7ae17a260"><a href="~/run-actions/0cfaac0e4511ead7ae17a260/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0cfaac0e4511ead7ae17a260/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
