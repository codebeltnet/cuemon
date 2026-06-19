---
uid: Cuemon.AspNetCore.Diagnostics.FaultDescriptorOptionsDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the `FaultDescriptorOptions` decorator extensions to try resolving an HTTP exception descriptor from a failure.

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
