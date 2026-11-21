---
uid: Cuemon.AspNetCore.Diagnostics.PreferredFaultDescriptor
example:
- *content
---

The following example demonstrates how to use the <xref cref="Cuemon.AspNetCore.Diagnostics.PreferredFaultDescriptor"/> enum to configure the error response format for the `FaultDescriptorFilter`.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;

        var preferredFormat = PreferredFaultDescriptor.ProblemDetails;
Console.WriteLine(preferredFormat);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4ce6c9aa63569288398737f8"><a href="~/run-actions/4ce6c9aa63569288398737f8/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4ce6c9aa63569288398737f8/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
