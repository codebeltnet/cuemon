---
uid: Cuemon.IO.AsyncStreamReaderOptions
example:
- *content
---

`AsyncStreamReaderOptions` configures encoding, preamble handling, and buffer size for asynchronous stream reading operations. This example creates a `MemoryStream` with UTF-8 preamble bytes followed by `"Hej Cuemon"` text, sets up options with `PreambleSequence.Remove` to strip the BOM, `Encoding = EncodingOptions.DefaultEncoding` for auto-detection, and `BufferSize = 4096`, then calls `ToEncodedStringAsync` via the decorator pattern. Console output shows the encoding web name (`utf-8`) and the decoded text with the preamble stripped, confirming correct preamble handling.

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
