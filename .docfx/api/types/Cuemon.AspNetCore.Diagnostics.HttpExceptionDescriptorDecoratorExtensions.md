---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptorDecoratorExtensions
example:
- *content
---

The following example demonstrates how to convert an `HttpExceptionDescriptor` to a `ProblemDetails` instance using the decorator extensions.

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
