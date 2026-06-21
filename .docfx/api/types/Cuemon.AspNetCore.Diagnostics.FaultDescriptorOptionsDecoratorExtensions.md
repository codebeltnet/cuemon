---
uid: Cuemon.AspNetCore.Diagnostics.FaultDescriptorOptionsDecoratorExtensions
example:
- *content
---

`FaultDescriptorOptionsDecoratorExtensions` provides extension methods on `Decorator.Enclose` for resolving HTTP exception descriptors from failure objects using `FaultDescriptorOptions`. This example creates a `FaultDescriptorOptions` instance and a `BadRequestException` as the failure input, then wraps the options with `Decorator.Enclose` and calls `TryResolveHttpExceptionDescriptor` with the failure and an `HttpContext`. The resolved descriptor's `StatusCode` is output as `400`, confirming the failure was correctly mapped to a `Bad Request` HTTP response.

```csharp
using System;
using Cuemon;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

namespace MyApp.Examples;

public class FaultDescriptorOptionsDecoratorExtensionsExample
{
    public void Demonstrate(HttpContext context)
    {
        var options = new FaultDescriptorOptions();
        var failure = new BadRequestException("Bad request");
        Decorator.Enclose(options).TryResolveHttpExceptionDescriptor(
            failure,
            context,
            descriptor => Console.WriteLine("Resolved descriptor"),
            out var descriptor);
        Console.WriteLine(descriptor.StatusCode); // 400
    }
}
```
