---
uid: Cuemon.AspNetCore.Http.PreconditionRequiredException
example:
- *content
---

The following example demonstrates how to use `PreconditionRequiredException` when a request requires conditional headers.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

var hasConditionHeader = false;
var exception = hasConditionHeader
    ? new PreconditionRequiredException()
    : new PreconditionRequiredException("Supply an If-Match header before retrying the update.");

Console.WriteLine(exception.Message);
Console.WriteLine(exception.StatusCode);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c0caa21ee002bb81f4cf786a"><a href="~/run-actions/c0caa21ee002bb81f4cf786a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c0caa21ee002bb81f4cf786a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
