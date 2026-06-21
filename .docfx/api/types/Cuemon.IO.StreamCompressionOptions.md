---
uid: Cuemon.IO.StreamCompressionOptions
example:
- *content
---

`StreamCompressionOptions` configures the compression level for synchronous stream compression operations. This example creates default options with `CompressionLevel.Optimal`, fast options with `CompressionLevel.Fastest`, and no-compression options for testing, then compresses sample data using `DeflateStream` with the fastest level. Console output shows the default compression level (`Optimal`), the original byte size of sample text, and the compressed byte size after deflate compression, demonstrating the compression ratio achieved.

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
