---
uid: Cuemon.IO.StreamReaderOptions
example:
- *content
---

`StreamReaderOptions` configures encoding, preamble handling, and buffer size for reading from streams via `StreamReader`. This example shows default options (UTF-8, `PreambleSequence.Remove`, `BufferSize = 81920`) and custom options for UTF-32 with `PreambleSequence.Keep` and `BufferSize = 4096`. A `StreamReader` is created using the custom options to read `"Hello, World!"` UTF-32 data from a `MemoryStream`, and the default preamble and buffer size values are also printed. Console output displays the default encoding name (`UTF-8`), preamble mode, buffer size, and the read content.

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon.IO;
using Cuemon.Text;

namespace MyApp.IO
{
    public class StreamReaderOptionsExample
    {
        public void Demonstrate()
        {
            // Default options: UTF-8 without BOM, 81920 buffer
            var defaultOptions = new StreamReaderOptions();
            Console.WriteLine($"Default encoding: {defaultOptions.Encoding.EncodingName}"); // UTF-8
            Console.WriteLine($"Default preamble: {defaultOptions.Preamble}"); // Remove
            Console.WriteLine($"Default buffer size: {defaultOptions.BufferSize}"); // 81920

            // Read a UTF-32 file with a smaller buffer
            var utf32Options = new StreamReaderOptions
            {
                Encoding = Encoding.UTF32,
                Preamble = PreambleSequence.Keep,
                BufferSize = 4096
            };

            // Create a StreamReader using these options
            byte[] data = Encoding.UTF32.GetBytes("Hello, World!");
            using var stream = new MemoryStream(data);
            using var reader = new StreamReader(stream, utf32Options.Encoding, false, utf32Options.BufferSize);

            string content = reader.ReadToEnd();
            Console.WriteLine($"Read content: {content}");

            // Options for reading with explicit BOM handling
            var bomOptions = new StreamReaderOptions
            {
                Encoding = Encoding.UTF8,
                Preamble = PreambleSequence.Keep
            };
            Console.WriteLine($"BOM preamble: {bomOptions.Preamble}"); // Keep

}}
}

```
