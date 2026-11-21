---
uid: Cuemon.Extensions.AspNetCore.Diagnostics.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add diagnostics middleware to the ASP.NET Core pipeline using the <xref:Cuemon.Extensions.AspNetCore.Diagnostics.ApplicationBuilderExtensions> class.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLogging();
services.AddServerTiming();
services.AddFaultDescriptorOptions();
using var provider = services.BuildServiceProvider();

var app = new ApplicationBuilder(provider);
app.UseServerTiming();
app.UseFaultDescriptorExceptionHandler();

Console.WriteLine("Server-Timing and fault descriptor middleware configured.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="72e8f97ae4d6693ccecee6c5"><a href="~/run-actions/72e8f97ae4d6693ccecee6c5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/72e8f97ae4d6693ccecee6c5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
