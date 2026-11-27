---
uid: Cuemon.AspNetCore.Http.InternalServerErrorException
example:
- *content
---

The following example demonstrates how to use `InternalServerErrorException` to represent an unexpected server-side failure in an exception handling middleware.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

var failure = new InvalidOperationException("The invoice pipeline failed to commit the transaction.");
var exception = new InternalServerErrorException(failure);

Console.WriteLine(exception.StatusCode);
Console.WriteLine(exception.InnerException?.Message);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="986b90c2da8ce04fad2885cd"><a href="~/run-actions/986b90c2da8ce04fad2885cd/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/986b90c2da8ce04fad2885cd/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
