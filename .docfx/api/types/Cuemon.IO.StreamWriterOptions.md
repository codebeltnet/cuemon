---
uid: Cuemon.IO.StreamWriterOptions
example:
- *content
---

`StreamWriterOptions` configures encoding, preamble handling, buffer size, format provider, and newline style when writing to streams via `StreamWriter`. This example sets up options with `AutoFlush = true`, `BufferSize = 256`, `Encoding = EncodingOptions.DefaultEncoding`, `Preamble = PreambleSequence.Remove`, `FormatProvider = CultureInfo.InvariantCulture`, and `NewLine = "\n"`, then uses `StreamFactory.Create` with a writer delegate to format a string with `Math.PI` to two decimal places. The resulting stream is read back as a string via the decorator pattern. Console output displays the trimmed formatted output `"Value: 3.14"`.

```csharp
// Program.cs
using System;
using System.Globalization;
using System.IO;
using Cuemon;
using Cuemon.IO;
using Cuemon.Text;

var options = new StreamWriterOptions
{
    AutoFlush = true,
    BufferSize = 256,
    Encoding = EncodingOptions.DefaultEncoding,
    Preamble = PreambleSequence.Remove,
    FormatProvider = CultureInfo.InvariantCulture,
    NewLine = "\n"
};

using Stream stream = StreamFactory.Create(writer =>
{
    string formatted = string.Format(options.FormatProvider, "Value: {0:F2}", Math.PI);
    writer.WriteLine(formatted);
}, setup =>
{
    setup.AutoFlush = options.AutoFlush;
    setup.BufferSize = options.BufferSize;
    setup.Encoding = options.Encoding;
    setup.Preamble = options.Preamble;
    setup.FormatProvider = options.FormatProvider;
    setup.NewLine = options.NewLine;
});

string output = Decorator.Enclose(stream).ToEncodedString(setup =>
{
    setup.Encoding = options.Encoding;
    setup.Preamble = options.Preamble;
    setup.LeaveOpen = true;
});

Console.WriteLine(output.Trim());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9467484df8998c5972543769"><a href="~/run-actions/9467484df8998c5972543769/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9467484df8998c5972543769/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
