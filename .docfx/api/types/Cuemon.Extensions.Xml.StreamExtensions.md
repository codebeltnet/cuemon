---
uid: Cuemon.Extensions.Xml.StreamExtensions
example:
- *content
---

`StreamExtensions` in the `Xml` namespace provides stream-based XML operations including `XmlReader` creation, indented copying, encoding detection, and namespace removal. This example loads an XML string into a `MemoryStream` and demonstrates four operations: `ToXmlReader` to parse element names, `CopyXmlStream` with `Indent = true` for pretty-printed XML output, `TryDetectXmlEncoding` to identify `UTF-8` encoding, and `RemoveXmlNamespaceDeclarations` to strip namespace prefixes from elements. Each operation repositions the stream to `Position = 0` before proceeding. Console output shows element names (`root`, `item`), the indented XML, the detected encoding name (`Unicode (UTF-8)`), and the namespace-cleaned content.

```csharp
using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Extensions.Xml;

namespace MyApp.Xml
{
    public class StreamExtensionsExample
    {
        public void Demonstrate()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><item>Value</item></root>";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

            // Convert an XML stream to an XmlReader
            using (var reader = stream.ToXmlReader())
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        Console.WriteLine(reader.Name);

            // Reset stream position for next demonstration
            stream.Position = 0;

            // Copy an XML stream with specified writer settings (e.g., indented output)
            using (var copy = stream.CopyXmlStream(o => o.Indent = true))
            {
                var indentedXml = new StreamReader(copy).ReadToEnd();
                Console.WriteLine(indentedXml); // Indented XML

            stream.Position = 0;

            // Detect XML encoding from the stream
            if (stream.TryDetectXmlEncoding(out var encoding))
            {
                Console.WriteLine(encoding.EncodingName); // "Unicode (UTF-8)"

            stream.Position = 0;

            // Remove XML namespace declarations from the stream
            using (var noNs = stream.RemoveXmlNamespaceDeclarations())
            {
                var cleaned = new StreamReader(noNs).ReadToEnd();
                Console.WriteLine(cleaned); // Elements without namespace prefixes

            stream.Dispose();

}}}}}}}}
}

```
