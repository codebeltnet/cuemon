---
uid: Cuemon.Extensions.IO.StreamExtensions
example:
- *content
---

`StreamExtensions` provides extension methods for `Stream` covering concatenation, encoding conversion, compression (GZip, Deflate, Brotli), and encoding detection. This example creates memory streams from `"Cue"` and `"mon"` and concatenates them with `Concat`, then converts the result to byte arrays, character arrays, and encoded strings both synchronously and asynchronously. It demonstrates compression round-trips for GZip, Deflate, and Brotli — each compressing a source stream, decompressing back, and reading the result — and also includes `TryDetectUnicodeEncoding` on a BOM-prefixed stream and `WriteAllAsync` for asynchronous writes. Console output confirms that all compressed round-trips preserve the original payload (`"gzip payload"`, `"deflate payload"`, `"brotli payload"`), that detected encoding is UTF-8, and that byte counts and string values agree across sync and async paths.

```csharp
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions.IO;

namespace MyApp.Examples
{
    public static class StreamExtensionsExample
    {
        public static async Task DemonstrateAsync()
        {
            using var first = CreateStream("Cue");
            using var second = CreateStream("mon");
            using var combined = first.Concat(second, options => options.LeaveOpen = true);

            var bytes = combined.ToByteArray();
            combined.Position = 0;
            var bytesAsync = await combined.ToByteArrayAsync();
            combined.Position = 0;
            var chars = combined.ToCharArray();
            combined.Position = 0;
            var text = combined.ToEncodedString();
            combined.Position = 0;
            var asyncText = await combined.ToEncodedStringAsync();

            using var bomStream = CreateStream("Hello with BOM", includePreamble: true);
            bomStream.TryDetectUnicodeEncoding(out var detectedEncoding);

            using var writable = new MemoryStream();
            await writable.WriteAllAsync(Encoding.UTF8.GetBytes("written asynchronously"));
            writable.Position = 0;

            using var gzipSource = CreateStream("gzip payload");
            using var gzipCompressed = gzipSource.CompressGZip();
            gzipCompressed.Position = 0;
            using var gzipDecompressed = gzipCompressed.DecompressGZip();
            gzipDecompressed.Position = 0;
            var gzipRoundTrip = gzipDecompressed.ToEncodedString();

            using var gzipAsyncSource = CreateStream("gzip async payload");
            using var gzipCompressedAsync = await gzipAsyncSource.CompressGZipAsync();
            gzipCompressedAsync.Position = 0;
            using var gzipDecompressedAsync = await gzipCompressedAsync.DecompressGZipAsync();
            gzipDecompressedAsync.Position = 0;
            var gzipAsyncRoundTrip = await gzipDecompressedAsync.ToEncodedStringAsync();

            using var deflateSource = CreateStream("deflate payload");
            using var deflateCompressed = deflateSource.CompressDeflate();
            deflateCompressed.Position = 0;
            using var deflateDecompressed = deflateCompressed.DecompressDeflate();
            deflateDecompressed.Position = 0;
            var deflateRoundTrip = deflateDecompressed.ToEncodedString();

            using var deflateAsyncSource = CreateStream("deflate async payload");
            using var deflateCompressedAsync = await deflateAsyncSource.CompressDeflateAsync();
            deflateCompressedAsync.Position = 0;
            using var deflateDecompressedAsync = await deflateCompressedAsync.DecompressDeflateAsync();
            deflateDecompressedAsync.Position = 0;
            var deflateAsyncRoundTrip = await deflateDecompressedAsync.ToEncodedStringAsync();

            using var brotliSource = CreateStream("brotli payload");
            using var brotliCompressed = brotliSource.CompressBrotli();
            brotliCompressed.Position = 0;
            using var brotliDecompressed = brotliCompressed.DecompressBrotli();
            brotliDecompressed.Position = 0;
            var brotliRoundTrip = brotliDecompressed.ToEncodedString();

            using var brotliAsyncSource = CreateStream("brotli async payload");
            using var brotliCompressedAsync = await brotliAsyncSource.CompressBrotliAsync();
            brotliCompressedAsync.Position = 0;
            using var brotliDecompressedAsync = await brotliCompressedAsync.DecompressBrotliAsync();
            brotliDecompressedAsync.Position = 0;
            var brotliAsyncRoundTrip = await brotliDecompressedAsync.ToEncodedStringAsync();

            Console.WriteLine(bytes.Length == bytesAsync.Length);
            Console.WriteLine(chars.Length);
            Console.WriteLine(text == asyncText);
            Console.WriteLine(detectedEncoding?.WebName);
            Console.WriteLine(writable.ToEncodedString());
            Console.WriteLine(gzipRoundTrip);
            Console.WriteLine(gzipAsyncRoundTrip);
            Console.WriteLine(deflateRoundTrip);
            Console.WriteLine(deflateAsyncRoundTrip);
            Console.WriteLine(brotliRoundTrip);
            Console.WriteLine(brotliAsyncRoundTrip);
        }

        private static MemoryStream CreateStream(string value, bool includePreamble = false)
        {
            var content = Encoding.UTF8.GetBytes(value);
            if (!includePreamble)
            {
                return new MemoryStream(content);
            }

            var preamble = Encoding.UTF8.GetPreamble();
            var buffer = new byte[preamble.Length + content.Length];
            Buffer.BlockCopy(preamble, 0, buffer, 0, preamble.Length);
            Buffer.BlockCopy(content, 0, buffer, preamble.Length, content.Length);
            return new MemoryStream(buffer);
        }
    }
}
```
