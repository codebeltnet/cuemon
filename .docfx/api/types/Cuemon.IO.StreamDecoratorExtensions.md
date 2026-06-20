---
uid: Cuemon.IO.StreamDecoratorExtensions
example:
- *content
---

`StreamDecoratorExtensions` provides extension methods on `Decorator.Enclose` for stream operations including byte array conversion, string encoding, compression (GZip, Deflate, Brotli), and copying. This example creates multiple `MemoryStream` instances with sample text and demonstrates `ToByteArray` and `ToByteArrayAsync`, `ToEncodedString` and `ToEncodedStringAsync`, `CopyStream` and `CopyStreamAsync`, `CompressGZip`/`DecompressGZip`, `CompressDeflate`/`DecompressDeflate`, `CompressBrotli`/`DecompressBrotli`, and `WriteAllAsync` — all via the decorator pattern. Console output confirms each operation's result, such as byte array lengths, decompressed strings matching the original content, and stream copy sizes.

```csharp
using System;
using Cuemon;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.IO;

namespace MyApp.IO;

public class StreamDecoratorExtensionsExample
{
    public async Task DemonstrateAsync()
    {
        // ---- InvokeToByteArray: convert stream to byte array ----
        var source0 = new MemoryStream(Encoding.UTF8.GetBytes("Invoke test."));
        Decorator.Enclose(source0).InvokeToByteArray();

        // ---- ToByteArray: convert stream to byte array ----
        var source1 = new MemoryStream(Encoding.UTF8.GetBytes("Hello, World!"));
        byte[] bytes = Decorator.Enclose(source1).ToByteArray();
        Console.WriteLine($"Byte array length: {bytes.Length}"); // 13

        // ---- ToByteArrayAsync: async version ----
        var source2 = new MemoryStream(Encoding.UTF8.GetBytes("Async bytes."));
        byte[] asyncBytes = await Decorator.Enclose(source2).ToByteArrayAsync();
        Console.WriteLine($"Async bytes length: {asyncBytes.Length}"); // 11

        // ---- ToEncodedString: stream to string ----
        var source3 = new MemoryStream(Encoding.UTF8.GetBytes("Read me as text."));
        string text = Decorator.Enclose(source3).ToEncodedString(setup =>
        {
            setup.LeaveOpen = true;
        });
        Console.WriteLine(text); // Read me as text.

        // ---- ToEncodedStringAsync: async stream to string ----
        source3.Position = 0;
        string textAsync = await Decorator.Enclose(source3).ToEncodedStringAsync(setup =>
        {
            setup.LeaveOpen = true;
        });
        Console.WriteLine(textAsync); // Read me as text.

        // ---- CopyStreamAsync: async copy to destination stream ----
        var source4 = new MemoryStream(Encoding.UTF8.GetBytes("Copy me."));
        using var destination = new MemoryStream();
        await Decorator.Enclose(source4).CopyStreamAsync(destination, bufferSize: 8192, changePosition: true);
        Console.WriteLine($"Destination length: {destination.Length}"); // 8

        // ---- CopyStream: synchronous copy ----
        var source4b = new MemoryStream(Encoding.UTF8.GetBytes("Sync copy."));
        using var dest4b = new MemoryStream();
        Decorator.Enclose(source4b).CopyStream(dest4b);
        Console.WriteLine($"Sync copy length: {dest4b.Length}"); // 9

        // ---- CompressGZip / DecompressGZip ----
        var source5 = new MemoryStream(Encoding.UTF8.GetBytes("GZip compress this."));
        using var gzipCompressed = Decorator.Enclose(source5).CompressGZip();
        Console.WriteLine($"GZip compressed size: {gzipCompressed.Length}");
        gzipCompressed.Position = 0;
        var gzipDecompressed = Decorator.Enclose(gzipCompressed).DecompressGZip();
        var gzipText = await Decorator.Enclose(gzipDecompressed).ToEncodedStringAsync();
        Console.WriteLine(gzipText); // GZip compress this.

        // ---- CompressGZipAsync / DecompressGZipAsync ----
        var source5b = new MemoryStream(Encoding.UTF8.GetBytes("Async GZip test."));
        var gzipCompressedAsync = await Decorator.Enclose(source5b).CompressGZipAsync();
        var gzipDecompressedAsync = await Decorator.Enclose(gzipCompressedAsync).DecompressGZipAsync();
        var gzipAsyncText = await Decorator.Enclose(gzipDecompressedAsync).ToEncodedStringAsync();
        Console.WriteLine(gzipAsyncText); // Async GZip test.

        // ---- CompressDeflate / DecompressDeflate (sync) ----
        var source6 = new MemoryStream(Encoding.UTF8.GetBytes("Deflate test."));
        using var deflated = Decorator.Enclose(source6).CompressDeflate();
        deflated.Position = 0;
        var deflatedResult = Decorator.Enclose(deflated).DecompressDeflate();
        var deflateText = await Decorator.Enclose(deflatedResult).ToEncodedStringAsync();
        Console.WriteLine(deflateText); // Deflate test.

        // ---- CompressDeflateAsync / DecompressDeflateAsync ----
        var source6b = new MemoryStream(Encoding.UTF8.GetBytes("Async Deflate."));
        var deflatedAsync = await Decorator.Enclose(source6b).CompressDeflateAsync();
        var deflatedDecompAsync = await Decorator.Enclose(deflatedAsync).DecompressDeflateAsync();
        var deflateAsyncText = await Decorator.Enclose(deflatedDecompAsync).ToEncodedStringAsync();
        Console.WriteLine(deflateAsyncText); // Async Deflate.

        // ---- ToByteArrayAsync / ToEncodedStringAsync (async byte/string conversion) ----
        var sourceBytes = new MemoryStream(Encoding.UTF8.GetBytes("Async byte conversion."));
        var bytesAsync = await Decorator.Enclose(sourceBytes).ToByteArrayAsync();
        Console.WriteLine(bytesAsync.Length); // 22

        sourceBytes.Position = 0;
        var strAsync = await Decorator.Enclose(sourceBytes).ToEncodedStringAsync();
        Console.WriteLine(strAsync); // Async byte conversion.

        // ---- CompressBrotli / DecompressBrotli (sync, netstandard2.1+ or net9.0+) ----
        var source7 = new MemoryStream(Encoding.UTF8.GetBytes("Brotli test."));
        var brotliCompressed = Decorator.Enclose(source7).CompressBrotli();
        brotliCompressed.Position = 0;
        var brotliDecompressed = Decorator.Enclose(brotliCompressed).DecompressBrotli();
        var brotliText = await Decorator.Enclose(brotliDecompressed).ToEncodedStringAsync();
        Console.WriteLine(brotliText); // Brotli test.

        // ---- CompressBrotliAsync / DecompressBrotliAsync ----
        var source7b = new MemoryStream(Encoding.UTF8.GetBytes("Async Brotli."));
        var brotliCompressedAsync = await Decorator.Enclose(source7b).CompressBrotliAsync();
        var brotliDecompressedAsync = await Decorator.Enclose(brotliCompressedAsync).DecompressBrotliAsync();
        var brotliAsyncText = await Decorator.Enclose(brotliDecompressedAsync).ToEncodedStringAsync();
        Console.WriteLine(brotliAsyncText); // Async Brotli.

        // ---- WriteAllAsync: write bytes to stream ----
        var target = new MemoryStream();
        byte[] data = Encoding.UTF8.GetBytes("Write this.");
        await Decorator.Enclose(target).WriteAllAsync(data);
        Console.WriteLine($"Written bytes: {target.Length}"); // 10

    }
}

```
