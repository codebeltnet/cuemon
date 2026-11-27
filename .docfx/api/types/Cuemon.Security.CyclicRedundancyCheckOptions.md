---
uid: Cuemon.Security.CyclicRedundancyCheckOptions
example:
- *content
---

The following example shows how to reuse one `CyclicRedundancyCheckOptions` instance when configuring a checksum implementation.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon;
using Cuemon.Security;

var options = new CyclicRedundancyCheckOptions
{
    ByteOrder = Endianness.LittleEndian,
    ReflectInput = true,
    ReflectOutput = true
};

var checksum = new CyclicRedundancyCheck32(setup: configured =>
{
    configured.ByteOrder = options.ByteOrder;
    configured.ReflectInput = options.ReflectInput;
    configured.ReflectOutput = options.ReflectOutput;
});

Console.WriteLine(options.ByteOrder);
Console.WriteLine(options.ReflectInput);
Console.WriteLine(options.ReflectOutput);
Console.WriteLine(checksum.ComputeHash(Encoding.ASCII.GetBytes("123456789")).ToHexadecimalString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="aae23689419b3461efdffd94"><a href="~/run-actions/aae23689419b3461efdffd94/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/aae23689419b3461efdffd94/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
