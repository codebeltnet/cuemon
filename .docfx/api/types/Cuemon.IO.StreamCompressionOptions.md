---
uid: Cuemon.IO.StreamCompressionOptions
example:
- *content
---

The following example demonstrates how to configure StreamCompressionOptions to control the compression level when compressing stream data.

```csharp
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Cuemon.IO;

namespace MyApp.IO
{
    public class StreamCompressionOptionsExample
    {
        public void Demonstrate()
        {
            // Default options: Optimal compression level
            var defaultOptions = new StreamCompressionOptions();
            Console.WriteLine($"Default compression level: {defaultOptions.Level}"); // Optimal

            // Fastest compression (less CPU, larger output)
            var fastOptions = new StreamCompressionOptions
            {
                Level = CompressionLevel.Fastest
            };

            // No compression (for testing)
            var noCompression = new StreamCompressionOptions
            {
                Level = CompressionLevel.NoCompression
            };

            // Compress some data
            var originalData = Encoding.UTF8.GetBytes("This is a test string that will be compressed.");
            using var source = new MemoryStream(originalData);
            using var compressed = new MemoryStream();
            using (var deflateStream = new DeflateStream(compressed, fastOptions.Level, leaveOpen: true))
            {
                source.CopyTo(deflateStream);

            Console.WriteLine($"Original size: {originalData.Length} bytes");
            Console.WriteLine($"Compressed size: {compressed.Length} bytes");

}}}
}

```
