---
uid: Cuemon.AspNetCore.Diagnostics.FaultDescriptorOptionsDecoratorExtensions
example:
- *content
---

`FaultDescriptorOptionsDecoratorExtensions` provides extension methods on `Decorator.Enclose` for resolving HTTP exception descriptors from failure objects using `FaultDescriptorOptions`. This example creates a `FaultDescriptorOptions` instance and a `BadRequestException` as the failure input, then wraps the options with `Decorator.Enclose` and calls `TryResolveHttpExceptionDescriptor` with the failure and an `HttpContext`. The resolved descriptor's `StatusCode` is output as `400`, confirming the failure was correctly mapped to a `Bad Request` HTTP response.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

var options = new FaultDescriptorOptions();
var failure = new BadRequestException("Bad request");
var context = new DefaultHttpContext();

Decorator.Enclose(options).TryResolveHttpExceptionDescriptor(
    failure,
    context,
    descriptor => Console.WriteLine("Resolved descriptor"),
    out var descriptor);

Console.WriteLine(descriptor.StatusCode);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9e0a6207d8d3862be1cf3da9"><a href="~/run-actions/9e0a6207d8d3862be1cf3da9/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9e0a6207d8d3862be1cf3da9/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
