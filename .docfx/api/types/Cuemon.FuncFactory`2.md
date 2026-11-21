---
uid: Cuemon.FuncFactory`2
example:
- *content
---

The following example demonstrates how to use <xref:Cuemon.FuncFactory`2> to encapsulate a function delegate and its arguments together for deferred execution.

```csharp
// Program.cs
using System;
using Cuemon;

var factory = new FuncFactory<MutableTuple<string, int>, string>(
    tuple => $"{tuple.Arg1}-{tuple.Arg2:D3}",
    new MutableTuple<string, int>("Item", 7));

string result = factory.ExecuteMethod();
var clone = (FuncFactory<MutableTuple<string, int>, string>)factory.Clone();

Console.WriteLine(result);
Console.WriteLine(clone.ExecuteMethod());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3fa40350e64c54af5412345c"><a href="~/run-actions/3fa40350e64c54af5412345c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3fa40350e64c54af5412345c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
