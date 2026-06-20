---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorDecoratorExtensions
example:
- *content
---

`HttpExceptionDescriptorDecoratorExtensions` provides extension methods on `Decorator.Enclose` for converting `HttpExceptionDescriptor` instances into ASP.NET Core `ProblemDetails` objects for structured API error responses. This example creates an `HttpExceptionDescriptor` from a `BadRequestException` with `CorrelationId`, `RequestId`, and `TraceId` set as context, then wraps it with `Decorator.Enclose` and calls `ToProblemDetails` with `FaultSensitivityDetails.None`. The resulting `ProblemDetails.Title` is printed to the console, showing the error output ready for serialization into an HTTP API response body.

```csharp
using System;
using Cuemon;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Diagnostics;

public static class HttpExceptionDescriptorDecoratorExtensionsExample
{
    public static void Demonstrate()
    {
        var descriptor = new HttpExceptionDescriptor(new BadRequestException())
        {
            CorrelationId = "corr-42",
            RequestId = "req-42",
            TraceId = "trace-42"
        };

        ProblemDetails problem = Decorator.Enclose(descriptor).ToProblemDetails(FaultSensitivityDetails.None);
        Console.WriteLine(problem.Title);
    }
}
```
