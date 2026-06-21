---
uid: Cuemon.IO.AsyncStreamCopyOptions
example:
- *content
---

`AsyncStreamCopyOptions` configures buffer size and stream lifetime for asynchronous stream copy operations. This example creates options with `BufferSize = 4096` and `LeaveOpen = true`, then copies UTF-8 string content from one `MemoryStream` to another using `Decorator.Enclose(source).CopyStreamAsync(destination, options.BufferSize, ...)`. The copied data is read back as a string and printed to confirm `"Copy me asynchronously."` was transferred intact, while the source stream remains open due to `LeaveOpen = true`.

```csharp
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.IO;

namespace Contoso.FileTransfers;

public sealed class AsyncStreamCopyOptionsExample
{
    public static async Task RunAsync()
    {
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
    }
}
```
