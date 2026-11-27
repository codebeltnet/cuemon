---
uid: Cuemon.AspNetCore.Http.NotAcceptableException
example:
- *content
---

The following example demonstrates how to instantiate `NotAcceptableException` with a descriptive message and inspect its HTTP 406 status code and reason phrase.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

var acceptHeader = "application/xml";
var exception = new NotAcceptableException($"The endpoint cannot produce '{acceptHeader}'.");

Console.WriteLine(exception.Message);
Console.WriteLine(exception.StatusCode);
Console.WriteLine(exception.ReasonPhrase);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="79cb40fe7c18161badb93c03"><a href="~/run-actions/79cb40fe7c18161badb93c03/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/79cb40fe7c18161badb93c03/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
