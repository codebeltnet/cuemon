---
uid: Cuemon.Extensions.ActionFactory
example:
- *content
---

The following example demonstrates how to create an `ActionFactory` from a callback with arguments and execute it. It shows both creating a factory with `Create` and invoking directly with `Invoke`.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Extensions;

Action<string, int> callback = (name, count) =>
{
    for (int i = 0; i < count; i++)
        Console.WriteLine(name);
};

var factory = ActionFactory.Create(callback, "Loop", 3);
factory.ExecuteMethod();

ActionFactory.Invoke(tuple => Console.WriteLine(tuple.Arg1), MutableTupleFactory.CreateOne("Direct invoke"));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9d886d7e4f68c003d3f4f6da"><a href="~/run-actions/9d886d7e4f68c003d3f4f6da/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9d886d7e4f68c003d3f4f6da/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
