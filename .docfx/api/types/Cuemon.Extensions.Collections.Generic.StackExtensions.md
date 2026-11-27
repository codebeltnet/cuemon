---
uid: Cuemon.Extensions.Collections.Generic.StackExtensions
example:
- *content
---

The following example shows how to use `TryPop` to safely remove items from a `Stack<string>`. It demonstrates popping until the stack is empty, after which `TryPop` returns `false` and sets the result to `null`.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon.Extensions.Collections.Generic;

var stack = new Stack<string>();
stack.Push("bottom");
stack.Push("top");

if (stack.TryPop(out string item))
{
    Console.WriteLine($"Popped: {item}");
}

stack.TryPop(out string next);
Console.WriteLine(next);

stack.TryPop(out string empty);
Console.WriteLine(empty == null);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ab4f0fb10ddff761784f333b"><a href="~/run-actions/ab4f0fb10ddff761784f333b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ab4f0fb10ddff761784f333b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
