---
uid: Cuemon.ByteArrayDecoratorExtensions
example:
- *content
---

`ByteArrayDecoratorExtensions` provides extension methods on `Decorator.Enclose` for converting byte arrays into strings, streams, and encoded text with configurable encoding. This example wraps a UTF-8 byte array and the ISO-8859-1-encoded `"Café"` bytes, then calls `ToEncodedString`, `ToStream`, and reads the stream via `StreamReader`. Key steps include setting encoding via the options delegate and verifying the stream is seekable and the correct length. Console output confirms the decoded strings match the original text, and the stream reports `Length = 13` with `CanSeek = True`.

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon;
using Cuemon.Text;

namespace MyApp
{
    public class ByteArrayDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // Create a byte array from a string
            byte[] data = Encoding.UTF8.GetBytes("Hello, World!");

            // Convert bytes to a string with default UTF-8 encoding
            string text = Decorator.Enclose(data).ToEncodedString();
            Console.WriteLine(text); // "Hello, World!"

            // Convert bytes to a string with specific encoding
            byte[] isoData = Encoding.GetEncoding("iso-8859-1").GetBytes("Café");
            string isoText = Decorator.Enclose(isoData).ToEncodedString(o =>
            {
                o.Encoding = Encoding.GetEncoding("iso-8859-1");
            });
            Console.WriteLine(isoText); // "Café"

            // Convert bytes to a seekable Stream
            using Stream stream = Decorator.Enclose(data).ToStream();
            Console.WriteLine(stream.Length); // 13
            Console.WriteLine(stream.CanSeek); // True

            // Read the stream back
            using var reader = new StreamReader(stream);
            string fromStream = reader.ReadToEnd();
            Console.WriteLine(fromStream); // "Hello, World!"

}}
}

```
