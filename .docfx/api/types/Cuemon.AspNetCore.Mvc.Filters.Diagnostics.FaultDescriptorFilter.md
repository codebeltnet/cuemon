---
uid: Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter
example:
- *content
---

The following example requires `Cuemon.AspNetCore.Mvc` and configures <xref cref="Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter"/> with `MvcFaultDescriptorOptions` (`ProblemDetails`, `MarkExceptionHandled = true`, and `FaultSensitivityDetails.Failure`). It sets up an `ExceptionContext` with a `ControllerActionDescriptor` and `NotFoundException`, then invokes `OnException`. As the outcome, the filter marks the exception handled and sets an `ExceptionDescriptorResult` with HTTP status 404.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using MyApp.Examples;

FaultDescriptorFilterExample.Demonstrate();

namespace MyApp.Examples
{
    public static class FaultDescriptorFilterExample
    {
        public static void Demonstrate()
        {
            var options = Options.Create(new MvcFaultDescriptorOptions
            {
                MarkExceptionHandled = true,
                FaultDescriptor = PreferredFaultDescriptor.ProblemDetails,
                SensitivityDetails = FaultSensitivityDetails.Failure
            });

            var filter = new FaultDescriptorFilter(options);

            var httpContext = new DefaultHttpContext();
            var actionDescriptor = new ControllerActionDescriptor
            {
                MethodInfo = typeof(FaultDescriptorFilterExample).GetMethod(nameof(SampleAction))!
            };
            var actionContext = new ActionContext(httpContext, new RouteData(), actionDescriptor);
            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = new NotFoundException("Order 42 was not found.")
            };

            filter.OnException(exceptionContext);

            Console.WriteLine($"ExceptionHandled: {exceptionContext.ExceptionHandled}");
            Console.WriteLine($"StatusCode: {httpContext.Response.StatusCode}");
            Console.WriteLine($"ResultType: {exceptionContext.Result?.GetType().Name}");
        }

        public static void SampleAction()
        {
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1ab7a24679d381a45a5ab845"><a href="~/run-actions/1ab7a24679d381a45a5ab845/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1ab7a24679d381a45a5ab845/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
