---
uid: Cuemon.IO.AsyncStreamCompressionOptions
example:
- *content
---

The following example demonstrates how to configure AsyncStreamCompressionOptions to control compression level and buffer size when compressing stream data asynchronously.

```csharp
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Cuemon.IO;

namespace MyApp.IO
{
    public class AsyncStreamCompressionOptionsExample
    {
        public async Task DemonstrateAsync()
        {
            // Default compression: Optimal balance of speed and size
            var defaultOptions = new AsyncStreamCompressionOptions();
            Console.WriteLine($"Default level: {defaultOptions.Level}"); // Optimal
            Console.WriteLine($"Default buffer size: {defaultOptions.BufferSize}"); // 81920

            // Fastest compression for CPU-sensitive scenarios
            var fastOptions = new AsyncStreamCompressionOptions
            {
                Level = CompressionLevel.Fastest,
                BufferSize = 4096
            };

            // Compress data using the configured options
            byte[] original = Encoding.UTF8.GetBytes("This is sample data that will be compressed using AsyncStreamCompressionOptions.");
            using var source = new MemoryStream(original);
            using var compressed = new MemoryStream();

            using (var deflateStream = new DeflateStream(compressed, fastOptions.Level, leaveOpen: true))
            {
                await source.CopyToAsync(deflateStream, fastOptions.BufferSize);

            Console.WriteLine($"Original size: {original.Length} bytes");
            Console.WriteLine($"Compressed size: {compressed.Length} bytes");

            // No compression level for testing or passthrough scenarios
            var noCompressionOptions = new AsyncStreamCompressionOptions
            {
                Level = CompressionLevel.NoCompression
            };
            Console.WriteLine($"No compression level: {noCompressionOptions.Level}");

}}}
}

```
