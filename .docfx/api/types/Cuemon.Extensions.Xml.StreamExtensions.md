---
uid: Cuemon.Extensions.Xml.StreamExtensions
example:
- *content
---

The following example demonstrates how to work with XML data from streams using StreamExtensions, including creating XmlReaders, copying with indented formatting, detecting encoding, and removing namespace declarations.

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
