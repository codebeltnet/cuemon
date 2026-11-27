---
uid: Cuemon.Extensions.Collections.Generic.QueueExtensions
example:
- *content
---

The following example shows how to use `TryPeek` to safely inspect the front of a `Queue<string>` without removing the item. It demonstrates that successive calls return the same element.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon.Extensions.Collections.Generic;

var queue = new Queue<string>();
queue.Enqueue("first");
queue.Enqueue("second");

if (queue.TryPeek(out string result))
{
    Console.WriteLine($"Peeked: {result}");
}

queue.TryPeek(out string same);
Console.WriteLine(same);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="68a5a54a36e76851c9832e5a"><a href="~/run-actions/68a5a54a36e76851c9832e5a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/68a5a54a36e76851c9832e5a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
