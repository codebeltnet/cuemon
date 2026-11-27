---
uid: Cuemon.AspNetCore.Mvc.Filters.Diagnostics.MvcFaultDescriptorOptions
example:
- *content
---

The following example configures <xref cref="Cuemon.AspNetCore.Mvc.Filters.Diagnostics.MvcFaultDescriptorOptions"/> for Problem Details responses and marks MVC exceptions as handled after the filter runs.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Diagnostics;

var options = new MvcFaultDescriptorOptions
{
    MarkExceptionHandled = true,
    FaultDescriptor = PreferredFaultDescriptor.ProblemDetails,
    SensitivityDetails = FaultSensitivityDetails.Failure
};

Console.WriteLine(options.MarkExceptionHandled);
Console.WriteLine(options.FaultDescriptor);
Console.WriteLine(options.HttpFaultResolvers.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a3ac9d6023b5dd27822edee5"><a href="~/run-actions/a3ac9d6023b5dd27822edee5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a3ac9d6023b5dd27822edee5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
