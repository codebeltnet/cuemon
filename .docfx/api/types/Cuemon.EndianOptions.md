---
uid: Cuemon.EndianOptions
example:
- *content
---

The following example demonstrates how to configure `EndianOptions` to explicitly set the byte order for binary data operations.

```csharp
// Program.cs
using System;
using Cuemon;

var options = new EndianOptions
{
    ByteOrder = Endianness.BigEndian
};

var byteOrder = options.ByteOrder;
var isSystemLittleEndian = BitConverter.IsLittleEndian;

Console.WriteLine($"Configured byte order: {byteOrder}");
Console.WriteLine($"System is little-endian: {isSystemLittleEndian}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="bc59d014ae74109ff069236f"><a href="~/run-actions/bc59d014ae74109ff069236f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/bc59d014ae74109ff069236f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
