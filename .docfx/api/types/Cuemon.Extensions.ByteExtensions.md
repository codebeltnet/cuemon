---
uid: Cuemon.Extensions.ByteExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.ByteExtensions> extension methods to convert byte arrays to various string representations and detect Unicode encoding.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Extensions;

byte[] data = { 0xEF, 0xBB, 0xBF, 0x48, 0x65, 0x6C, 0x6C, 0x6F };
string text = data.ToEncodedString();
string hex = data.ToHexadecimalString();
string binary = data.ToBinaryString();
string base64 = data.ToBase64String();
string urlBase64 = data.ToUrlEncodedBase64String();

data.TryDetectUnicodeEncoding(out Encoding detectedEncoding);

Console.WriteLine(text);
Console.WriteLine(hex);
Console.WriteLine(binary);
Console.WriteLine(base64);
Console.WriteLine(urlBase64);
Console.WriteLine(detectedEncoding?.WebName ?? "unknown");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6dee641f323f5722009b2fd5"><a href="~/run-actions/6dee641f323f5722009b2fd5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6dee641f323f5722009b2fd5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
