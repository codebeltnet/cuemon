---
uid: Cuemon.Extensions.FuncFactory
example:
- *content
---

The following example demonstrates how to create a `FuncFactory` from a delegate and execute it to produce a result. It shows both creating a factory with `Create` and invoking directly with `Invoke` using a mutable tuple.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Extensions;

Func<int, int, string> formatter = (x, y) => $"{x} + {y} = {x + y}";

var factory = FuncFactory.Create(formatter, 3, 4);
string result = factory.ExecuteMethod();

Console.WriteLine(result);

int sum = FuncFactory.Invoke<MutableTuple<int, int>, int>(tuple => tuple.Arg1 + tuple.Arg2, MutableTupleFactory.CreateTwo(10, 20));
Console.WriteLine(sum);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="af07d7e2075dd585d241c990"><a href="~/run-actions/af07d7e2075dd585d241c990/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/af07d7e2075dd585d241c990/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
