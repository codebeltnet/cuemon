---
uid: Cuemon.AspNetCore.Http.UnsupportedMediaTypeException
example:
- *content
---

The following example demonstrates how to return an `UnsupportedMediaTypeException` when the API receives an unsupported file upload format.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

try
{
    throw new UnsupportedMediaTypeException("text/html");
}
catch (UnsupportedMediaTypeException ex)
{
    Console.WriteLine(ex.StatusCode); // 415
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="36718f58d3842855ac0ecc47"><a href="~/run-actions/36718f58d3842855ac0ecc47/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/36718f58d3842855ac0ecc47/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
