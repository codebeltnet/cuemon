---
uid: Cuemon.Extensions.Text.EncodingOptionsExtensions
example:
- *content
---

The following example demonstrates detecting the Unicode encoding of a byte array or stream using the <xref:Cuemon.Extensions.Text.EncodingOptionsExtensions.DetectUnicodeEncoding(Cuemon.Text.IEncodingOptions,System.Byte[])> and <xref:Cuemon.Extensions.Text.EncodingOptionsExtensions.DetectUnicodeEncoding(Cuemon.Text.IEncodingOptions,System.IO.Stream)> extension methods.

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon.Extensions.Text;
using Cuemon.Text;

namespace MyApp.Examples;

public class EncodingOptionsExtensionsExample
{
    public static void Main()
    {
        // Create an EncodingOptions instance with a fallback encoding
        var options = new EncodingOptions { Encoding = Encoding.UTF8 };

        // Byte array with UTF-8 BOM
        byte[] utf8WithBom = { 0xEF, 0xBB, 0xBF, 0x48, 0x65, 0x6C, 0x6C, 0x6F };

        // Detect encoding from bytes
        Encoding detected = options.DetectUnicodeEncoding(utf8WithBom);
        Console.WriteLine($"Detected encoding from bytes: {detected.EncodingName}");

        // Stream with UTF-16 LE BOM
        using var stream = new MemoryStream();
        byte[] utf16Bom = { 0xFF, 0xFE, 0x48, 0x00, 0x65, 0x00, 0x6C, 0x00 };
        stream.Write(utf16Bom, 0, utf16Bom.Length);
        stream.Position = 0;

        Encoding streamEncoding = options.DetectUnicodeEncoding(stream);
        Console.WriteLine($"Detected encoding from stream: {streamEncoding.EncodingName}");

        // When no BOM is present, the fallback encoding is returned
        byte[] noBom = { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
        Encoding fallback = options.DetectUnicodeEncoding(noBom);
        Console.WriteLine($"Fallback encoding: {fallback.EncodingName}");

}
}

```
