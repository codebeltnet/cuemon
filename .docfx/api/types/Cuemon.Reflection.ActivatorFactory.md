---
uid: Cuemon.Reflection.ActivatorFactory
example:
- *content
---

The following example shows how to create instances of types at runtime using `ActivatorFactory`. It creates a `StringBuilder` with no arguments and a `DateTime` with constructor arguments, then prints each result.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Reflection;

var sb = ActivatorFactory.CreateInstance<StringBuilder>();
sb.Append("Hello from activator");
Console.WriteLine(sb.ToString());

var dt = ActivatorFactory.CreateInstance<int, int, int, DateTime>(2025, 12, 1);
Console.WriteLine($"Created: {dt:yyyy-MM-dd}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="36a7fa492f2fe4d8b40fa625"><a href="~/run-actions/36a7fa492f2fe4d8b40fa625/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/36a7fa492f2fe4d8b40fa625/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
