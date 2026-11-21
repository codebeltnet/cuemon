---
uid: Cuemon.Endianness
example:
- *content
---

The following example demonstrates how to use `Endianness` to specify byte order when configuring `EndianOptions`.

```csharp
// Program.cs
using Cuemon;
using System;

var options = new EndianOptions
{
    ByteOrder = Endianness.BigEndian
};

Console.WriteLine(options.ByteOrder == Endianness.BigEndian);
// outputs: True
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="db20c1ea9d6c5131581ff7f6"><a href="~/run-actions/db20c1ea9d6c5131581ff7f6/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/db20c1ea9d6c5131581ff7f6/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
