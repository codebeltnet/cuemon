---
uid: Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter
example:
- *content
---

The following example creates <xref cref="Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter"/> directly from configured packet-local options.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Diagnostics;
using Microsoft.Extensions.Options;

var options = Options.Create(new MvcFaultDescriptorOptions
{
    MarkExceptionHandled = true,
    FaultDescriptor = PreferredFaultDescriptor.ProblemDetails,
    SensitivityDetails = FaultSensitivityDetails.Failure
});

var filter = new FaultDescriptorFilter(options);

Console.WriteLine(filter.Options.MarkExceptionHandled);
Console.WriteLine(filter.Options.FaultDescriptor);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c095b428e5385f0df6d3edc9"><a href="~/run-actions/c095b428e5385f0df6d3edc9/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c095b428e5385f0df6d3edc9/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
