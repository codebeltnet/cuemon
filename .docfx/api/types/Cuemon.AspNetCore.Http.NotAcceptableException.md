---
uid: Cuemon.AspNetCore.Http.NotAcceptableException
example:
- *content
---

The following example demonstrates how to configure an ASP.NET Core MVC filter that returns a `NotAcceptableException`.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

var acceptHeader = "application/xml";
var exception = new NotAcceptableException($"The endpoint cannot produce '{acceptHeader}'.");

Console.WriteLine(exception.Message);
Console.WriteLine(exception.StatusCode);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="965af4dc34598f2c8f4f0260"><a href="~/run-actions/965af4dc34598f2c8f4f0260/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/965af4dc34598f2c8f4f0260/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
