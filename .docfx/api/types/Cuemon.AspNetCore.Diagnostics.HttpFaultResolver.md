---
uid: Cuemon.AspNetCore.Diagnostics.HttpFaultResolver
example:
- *content
---

The following example demonstrates how to create an HTTP fault resolver that maps exceptions to HTTP error responses.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;

var resolver = new HttpFaultResolver(
    exception => exception is ArgumentNullException,
    exception => new HttpExceptionDescriptor(exception, 400, null, "Argument was null."));

if (resolver.TryResolveFault(new ArgumentNullException("value"), out var descriptor))
{
    Console.WriteLine(descriptor.StatusCode); // 400
}

var resolved = resolver.TryResolveFault(new InvalidOperationException(), out _);
Console.WriteLine(resolved); // False
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9c67f469c3acad2b059d54bf"><a href="~/run-actions/9c67f469c3acad2b059d54bf/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9c67f469c3acad2b059d54bf/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
