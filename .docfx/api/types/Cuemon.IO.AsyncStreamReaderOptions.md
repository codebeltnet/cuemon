---
uid: Cuemon.IO.AsyncStreamReaderOptions
example:
- *content
---

The following example demonstrates how to configure AsyncStreamReaderOptions to control encoding, preamble handling, and buffer size when reading stream content asynchronously.

```csharp
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.IO;
using Cuemon.Text;

namespace Contoso.Imports;

public sealed class AsyncStreamReaderOptionsExample
{
    public static async Task RunAsync()
    {
        byte[] payload = Encoding.UTF8.GetPreamble()
            .Concat(Encoding.UTF8.GetBytes("Hej Cuemon"))
            .ToArray();

        using var stream = new MemoryStream(payload);

        var options = new AsyncStreamReaderOptions
        {
            BufferSize = 4096,
            Encoding = EncodingOptions.DefaultEncoding,
            Preamble = PreambleSequence.Remove,
            LeaveOpen = true
        };

        string text = await Decorator.Enclose(stream).ToEncodedStringAsync(setup =>
        {
            setup.BufferSize = options.BufferSize;
            setup.Encoding = options.Encoding;
            setup.Preamble = options.Preamble;
            setup.LeaveOpen = options.LeaveOpen;
        });

        Console.WriteLine($"{options.Encoding.WebName}:{text}");
    }
}
```
