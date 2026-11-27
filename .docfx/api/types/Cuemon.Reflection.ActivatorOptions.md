---
uid: Cuemon.Reflection.ActivatorOptions
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Reflection.ActivatorOptions"/> with <see cref="ActivatorFactory.CreateInstance{T,TInstance}"/> to customize object creation with specific binding flags.

```csharp
// Program.cs
using System;
using System.Reflection;
using Cuemon.Reflection;

// Direct instantiation of ActivatorOptions
var options = new ActivatorOptions
{
    Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance
};

// Create a Uri instance using a factory with explicit binding flags
var uri = ActivatorFactory.CreateInstance<string, Uri>("http://example.com", o =>
{
    o.Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
});

Console.WriteLine(uri.Host);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4fc0d41824ba823ed0d833af"><a href="~/run-actions/4fc0d41824ba823ed0d833af/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4fc0d41824ba823ed0d833af/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
