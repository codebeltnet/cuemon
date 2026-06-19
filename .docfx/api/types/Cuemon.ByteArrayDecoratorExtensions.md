---
uid: Cuemon.ByteArrayDecoratorExtensions
example:
- *content
---

The following example shows how to extend `byte[]` with `ByteArrayDecoratorExtensions` methods to convert byte arrays into encoded strings and seekable streams.

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
