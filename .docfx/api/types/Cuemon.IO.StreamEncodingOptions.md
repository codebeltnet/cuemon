---
uid: Cuemon.IO.StreamEncodingOptions
example:
- *content
---

The following example demonstrates how to configure StreamEncodingOptions for preamble handling and encoding detection when reading stream content as strings.

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon;
using Cuemon.IO;
using Cuemon.Text;

namespace MyApp.IO;

public class StreamEncodingOptionsExample
{
    public void Demonstrate()
    {
        // Directly instantiate and use StreamEncodingOptions
        var defaultOptions = new StreamEncodingOptions();
        Console.WriteLine($"Default preamble handling: {defaultOptions.Preamble}");

        // Create a stream with UTF-32 encoded content including a BOM
        var text = "Hello with BOM!";
        var preamble = Encoding.UTF32.GetPreamble();
        var encoded = Encoding.UTF32.GetBytes(text);

        var stream = new MemoryStream(preamble.Length + encoded.Length);
        stream.Write(preamble, 0, preamble.Length);
        stream.Write(encoded, 0, encoded.Length);
        stream.Position = 0;

        // Read the stream as a string using StreamReaderOptions (which inherits StreamEncodingOptions)
        // This auto-detects the encoding from the BOM and removes the preamble from the output
        string result = Decorator.Enclose(stream).ToEncodedString(setup =>
        {
            setup.Encoding = EncodingOptions.DefaultEncoding; // auto-detect from BOM
            setup.Preamble = PreambleSequence.Remove;        // strip the BOM from output
            setup.LeaveOpen = true;                          // keep stream open for reuse
        });

        Console.WriteLine($"Read: {result}"); // Hello with BOM!

        // Read again, this time keeping the BOM preamble
        stream.Position = 0;
        string withBom = Decorator.Enclose(stream).ToEncodedString(setup =>
        {
            setup.Encoding = Encoding.UTF32;
            setup.Preamble = PreambleSequence.Keep;
            setup.LeaveOpen = false; // let the extension dispose the stream
        });

        Console.WriteLine($"With BOM length: {withBom.Length}"); // includes BOM bytes
        Console.WriteLine($"Stream disposed: {!stream.CanRead}"); // True

}
}

```
