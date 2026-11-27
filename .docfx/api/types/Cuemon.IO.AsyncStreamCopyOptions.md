---
uid: Cuemon.IO.AsyncStreamCopyOptions
example:
- *content
---

`AsyncStreamCopyOptions` configures buffer size and stream lifetime for asynchronous stream copy operations. This example creates options with `BufferSize = 4096` and `LeaveOpen = true`, then copies UTF-8 string content from one `MemoryStream` to another using `Decorator.Enclose(source).CopyStreamAsync(destination, options.BufferSize, ...)`. The copied data is read back via `ToByteArrayAsync` with `setup.LeaveOpen = options.LeaveOpen`, confirming `"Copy me asynchronously."` was transferred intact while `destination` remains open (`CanRead = True`).

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.IO;

var options = new AsyncStreamCopyOptions
{
    BufferSize = 4096,
    LeaveOpen = true
};

using var source = new MemoryStream(Encoding.UTF8.GetBytes("Copy me asynchronously."));
using var destination = new MemoryStream();

await Decorator.Enclose(source).CopyStreamAsync(destination, options.BufferSize, changePosition: true, ct: CancellationToken.None);

byte[] copied = await Decorator.Enclose(destination).ToByteArrayAsync(setup =>
{
    setup.BufferSize = options.BufferSize;
    setup.LeaveOpen = options.LeaveOpen;
});

Console.WriteLine(Encoding.UTF8.GetString(copied));
Console.WriteLine($"Destination stream is still open: {destination.CanRead}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e07c5fa5a31b36f38cecbd65"><a href="~/run-actions/e07c5fa5a31b36f38cecbd65/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e07c5fa5a31b36f38cecbd65/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
