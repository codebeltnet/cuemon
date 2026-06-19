---
uid: Cuemon.IO.StreamWriterOptions
example:
- *content
---

The following example demonstrates how to configure StreamWriterOptions to control encoding, preamble handling, buffer size, and formatting when writing to streams.

```csharp
using System;
using System.Globalization;
using System.IO;
using Cuemon;
using Cuemon.IO;
using Cuemon.Text;

namespace Contoso.Reporting;

public sealed class StreamWriterOptionsExample
{
    public static void Run()
    {
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
    }
}
```
