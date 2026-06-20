---
uid: Cuemon.Threading.AsyncPatterns
example:
- *content
---

`AsyncPatterns` provides safe asynchronous patterns for resource management, including `SafeInvokeAsync` that ensures proper disposal of `IDisposable` resources (CA2000 compliant). This example calls `SafeInvokeAsync` with a factory delegate creating a `MemoryStream`, an invocation delegate that writes `"Cuemon"` bytes to it and returns the stream, and a `CancellationToken`. The resulting stream is read back via `StreamReader` to confirm the content, and the `AsyncPatterns.Use` sentinel is compared with itself to verify static reference identity. Console output displays `"Cuemon"` and `True` for the reference comparison.

```csharp
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Contoso.Streaming;

public sealed class AsyncPatternsExample
{
    public static async Task RunAsync()
    {
        var result = await AsyncPatterns.SafeInvokeAsync(
            () => new MemoryStream(),
            async (stream, ct) =>
            {
                byte[] buffer = Encoding.UTF8.GetBytes("Cuemon");
                await stream.WriteAsync(buffer, 0, buffer.Length, ct);
                stream.Position = 0;
                return stream;
            },
            ct: CancellationToken.None);

        using (result)
        using (var reader = new StreamReader(result, Encoding.UTF8, true, 1024, true))
        {
            Console.WriteLine(await reader.ReadToEndAsync());
        }

        Console.WriteLine(ReferenceEquals(AsyncPatterns.Use, AsyncPatterns.Use));
    }
}
```
