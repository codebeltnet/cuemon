---
uid: Cuemon.AspNetCore.Mvc.GoneResult
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.GoneResult"/> can signal that a resource has been permanently removed.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc;

var result = new GoneResult();

Console.WriteLine(result.StatusCode);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c8da50a994d379136eec9afd"><a href="~/run-actions/c8da50a994d379136eec9afd/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c8da50a994d379136eec9afd/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
