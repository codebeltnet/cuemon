---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorDecoratorExtensions
example:
- *content
---

`HttpExceptionDescriptorDecoratorExtensions` provides extension methods on `Decorator.Enclose` for converting `HttpExceptionDescriptor` instances into ASP.NET Core `ProblemDetails` objects for structured API error responses. This example creates an `HttpExceptionDescriptor` from a `BadRequestException` with `CorrelationId`, `RequestId`, and `TraceId` set as context, then wraps it with `Decorator.Enclose` and calls `ToProblemDetails` with `FaultSensitivityDetails.None`. The resulting `ProblemDetails.Title` is printed to the console, showing the error output ready for serialization into an HTTP API response body.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cuemon.AspNetCore.Diagnostics;

var descriptor = new HttpExceptionDescriptor(new BadRequestException())
{
    CorrelationId = "corr-42",
    RequestId = "req-42",
    TraceId = "trace-42"
};

ProblemDetails problem = Decorator.Enclose(descriptor).ToProblemDetails(FaultSensitivityDetails.None);
Console.WriteLine(problem.Title);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e028d11cd6c2e90b7827f043"><a href="~/run-actions/e028d11cd6c2e90b7827f043/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e028d11cd6c2e90b7827f043/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
