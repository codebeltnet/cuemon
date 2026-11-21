---
uid: Cuemon.AspNetCore.Diagnostics.FaultDescriptorOptions
example:
- *content
---

The following example demonstrates how to configure fault descriptor options for structured error responses.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;

var options = new FaultDescriptorOptions
{
    SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace,
    RootHelpLink = new Uri("https://example.com/errors")
};

Console.WriteLine(options.SensitivityDetails);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f7d0d7179789e5fea4574199"><a href="~/run-actions/f7d0d7179789e5fea4574199/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f7d0d7179789e5fea4574199/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
