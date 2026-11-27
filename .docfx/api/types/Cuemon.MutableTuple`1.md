---
uid: Cuemon.MutableTuple`1
example:
- *content
---

The following example shows how to create a single-argument `MutableTuple<T>` and access its value. It demonstrates both reading and updating the `Arg1` property.

```csharp
// Program.cs
using System;
using Cuemon;

var tuple = new MutableTuple<string>("example");
Console.WriteLine(tuple.Arg1); // "example"

tuple.Arg1 = "updated";
Console.WriteLine(tuple.Arg1); // "updated"
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e8b4a308a70933793d69cfa7"><a href="~/run-actions/e8b4a308a70933793d69cfa7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e8b4a308a70933793d69cfa7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
