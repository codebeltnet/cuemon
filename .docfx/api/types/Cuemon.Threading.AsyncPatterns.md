---
uid: Cuemon.Threading.AsyncPatterns
example:
- *content
---

The following example demonstrates how to use `AsyncPatterns` for safe disposal of `IDisposable` resources (CA2000) in asynchronous workflows.

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
