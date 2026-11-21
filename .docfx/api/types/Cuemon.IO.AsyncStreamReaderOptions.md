---
uid: Cuemon.IO.AsyncStreamReaderOptions
example:
- *content
---

`AsyncStreamReaderOptions` configures encoding, preamble handling, and buffer size for asynchronous stream reading operations. This example creates a `MemoryStream` with UTF-8 preamble bytes followed by `"Hej Cuemon"` text, sets up options with `PreambleSequence.Remove` to strip the BOM, `Encoding = EncodingOptions.DefaultEncoding` for auto-detection, and `BufferSize = 4096`, then calls `ToEncodedStringAsync` via the decorator pattern. Console output shows the encoding web name (`utf-8`) and the decoded text with the preamble stripped, confirming correct preamble handling.

```csharp
// Program.cs
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.IO;
using Cuemon.Text;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0047075140e13ef7255341b7"><a href="~/run-actions/0047075140e13ef7255341b7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0047075140e13ef7255341b7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
