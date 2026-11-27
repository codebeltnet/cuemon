---
uid: Cuemon.IO.StreamCopyOptions
example:
- *content
---

`StreamCopyOptions` configures buffer size and whether the source stream remains open after synchronous copy operations. This example creates custom options with `BufferSize = 4096` and `LeaveOpen = true`, converts a `MemoryStream` to a byte array via the decorator pattern with these options, then confirms the source stream is still readable (`CanRead = True`). It also demonstrates the default behavior where `LeaveOpen = false` disposes the source stream after copying, showing `CanRead = False`. Console output displays the byte count (`25`), the decoded string (`"Hello, StreamCopyOptions!"`), and the stream's open/disposed state.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using Cuemon;
using Cuemon.IO;

// Create and use StreamCopyOptions directly
var customOptions = new StreamCopyOptions { BufferSize = 4096, LeaveOpen = true };
Console.WriteLine($"Buffer size: {customOptions.BufferSize}, Leave open: {customOptions.LeaveOpen}");

// Create a memory stream with some data
var source = new MemoryStream(Encoding.UTF8.GetBytes("Hello, StreamCopyOptions!"));

// Convert the stream to a byte array using custom StreamCopyOptions
byte[] bytes = Decorator.Enclose(source).ToByteArray(setup =>
{
    setup.BufferSize = 4096;
    setup.LeaveOpen = true;
});

Console.WriteLine($"Read {bytes.Length} bytes"); // 25
Console.WriteLine(Encoding.UTF8.GetString(bytes)); // Hello, StreamCopyOptions!
Console.WriteLine($"Source stream is still open: {source.CanRead}"); // True

// Without LeaveOpen, the source stream is automatically disposed after reading
var temp = new MemoryStream(Encoding.UTF8.GetBytes("Temporary data."));
byte[] tempBytes = Decorator.Enclose(temp).ToByteArray(); // disposes 'temp'
Console.WriteLine(Encoding.UTF8.GetString(tempBytes)); // Temporary data.
Console.WriteLine($"Stream was disposed: {!temp.CanRead}"); // True

// Using default options (BufferSize = 81920, LeaveOpen = false)
var data = new MemoryStream(Encoding.UTF8.GetBytes("Default options."));
byte[] result = Decorator.Enclose(data).ToByteArray();
Console.WriteLine(Encoding.UTF8.GetString(result)); // Default options.
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f634fbe03d02d0d4c89b0814"><a href="~/run-actions/f634fbe03d02d0d4c89b0814/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f634fbe03d02d0d4c89b0814/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
