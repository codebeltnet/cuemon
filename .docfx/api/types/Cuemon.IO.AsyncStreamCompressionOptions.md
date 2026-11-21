---
uid: Cuemon.IO.AsyncStreamCompressionOptions
example:
- *content
---

`AsyncStreamCompressionOptions` configures compression level and buffer size for asynchronous stream compression operations. This example creates default options with `CompressionLevel.Optimal` and an `81920` buffer size, fast options with `CompressionLevel.Fastest` and a `4096` buffer for CPU-sensitive scenarios, and no-compression options for testing or passthrough use. Key steps include compressing sample data using `DeflateStream` with the configured options and comparing original vs. compressed byte sizes. Console output shows the default level (`Optimal`), buffer size, and the compression ratio between original and compressed sizes.

```csharp
// Program.cs
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Cuemon.IO;

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

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="fc5bc2b379d6ad6e5386b6b5"><a href="~/run-actions/fc5bc2b379d6ad6e5386b6b5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/fc5bc2b379d6ad6e5386b6b5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
