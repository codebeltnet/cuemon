---
uid: Cuemon.AspNetCore.Mvc.ExceptionDescriptorResult
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.ExceptionDescriptorResult"/> can return either an <xref cref="Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptor"/> or ASP.NET Core <xref cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/>.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

var descriptorResult = new ExceptionDescriptorResult(
    new HttpExceptionDescriptor(new BadRequestException("City name is required.")));

var descriptor = (HttpExceptionDescriptor)descriptorResult.Value;
Console.WriteLine(descriptor.StatusCode);

var problemResult = new ExceptionDescriptorResult(
    new ProblemDetails { Title = "Validation failed." });

Console.WriteLine(problemResult.Value is IDecorator<ProblemDetails>);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="af9a275d7cff798d2c928dae"><a href="~/run-actions/af9a275d7cff798d2c928dae/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/af9a275d7cff798d2c928dae/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
