---
uid: Cuemon.AspNetCore.Mvc.ExceptionDescriptorResult
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.ExceptionDescriptorResult"/> can return either an <xref cref="Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptor"/> or ASP.NET Core <xref cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/>.

```csharp
using System;
using Cuemon;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Examples;

public static class ExceptionDescriptorResultExample
{
    public static void Demonstrate()
    {
        var descriptorResult = new ExceptionDescriptorResult(
            new HttpExceptionDescriptor(new BadRequestException("City name is required.")));

        var descriptor = (HttpExceptionDescriptor)descriptorResult.Value;
        Console.WriteLine(descriptor.StatusCode);

        var problemResult = new ExceptionDescriptorResult(
            new ProblemDetails { Title = "Validation failed." });

        Console.WriteLine(problemResult.Value is IDecorator<ProblemDetails>);
    }
}
```
