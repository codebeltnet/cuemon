---
uid: Cuemon.IO.AsyncStreamCopyOptions
example:
- *content
---

The following example demonstrates how to configure AsyncStreamCopyOptions to control buffer size and stream lifetime when copying data asynchronously between streams.

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
