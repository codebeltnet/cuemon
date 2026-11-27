---
uid: Cuemon.AspNetCore.Mvc.CacheableObjectResultOptions`1
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Mvc.CacheableObjectResultOptions{T}"/> to enable HTTP caching (ETag and Last-Modified) for a response model.

```csharp
// Program.cs
using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.AspNetCore.Mvc;

var options = new CacheableObjectResultOptions<string>
{
    ChecksumProvider = value => SHA256.HashData(Encoding.UTF8.GetBytes(value)),
    WeakChecksumProvider = _ => false,
    TimestampProvider = _ => DateTime.UtcNow,
    ChangedTimestampProvider = _ => DateTime.UtcNow
};

options.ValidateOptions();

var data = "hello-world";
byte[] checksum = options.ChecksumProvider(data);
Console.WriteLine($"Checksum length: {checksum.Length}"); // 32 bytes (SHA256)
Console.WriteLine($"Created: {options.TimestampProvider(data)}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="34dd40245556c272d2dd3d26"><a href="~/run-actions/34dd40245556c272d2dd3d26/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/34dd40245556c272d2dd3d26/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
