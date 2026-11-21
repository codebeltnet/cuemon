---
uid: Cuemon.Extensions.Xml.StreamExtensions
example:
- *content
---

`StreamExtensions` in the `Xml` namespace provides stream-based XML operations including `XmlReader` creation, indented copying, encoding detection, and namespace removal. This example loads an XML string into a `MemoryStream` and demonstrates four operations: `ToXmlReader` to parse element names, `CopyXmlStream` with `Indent = true` for pretty-printed XML output, `TryDetectXmlEncoding` to identify `UTF-8` encoding, and `RemoveXmlNamespaceDeclarations` to strip namespace prefixes from elements. Each operation repositions the stream to `Position = 0` before proceeding. Console output shows element names (`root`, `item`), the indented XML, the detected encoding name (`Unicode (UTF-8)`), and the namespace-cleaned content.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Extensions.Xml;
using MyApp.Xml;

new StreamExtensionsExample().Demonstrate();

namespace MyApp.Xml
{
    public class StreamExtensionsExample
    {
        public void Demonstrate()
        {
            const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><item>Value</item></root>";

            using (var reader = CreateStream(xml).ToXmlReader())
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        Console.WriteLine(reader.Name);
                    }
                }
            }

            using (var copy = CreateStream(xml).CopyXmlStream(o => o.Indent = true))
            using (var streamReader = new StreamReader(copy))
            {
                Console.WriteLine(streamReader.ReadToEnd());
            }

            using (var encodingStream = CreateStream(xml))
            {
                if (encodingStream.TryDetectXmlEncoding(out var encoding))
                {
                    Console.WriteLine(encoding.EncodingName);
                }
            }

            using (var noNamespace = CreateStream(xml).RemoveXmlNamespaceDeclarations())
            using (var streamReader = new StreamReader(noNamespace))
            {
                Console.WriteLine(streamReader.ReadToEnd());
            }
        }

        private static MemoryStream CreateStream(string xml)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(xml));
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b418c59f6a61ed2f5745e0ec"><a href="~/run-actions/b418c59f6a61ed2f5745e0ec/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b418c59f6a61ed2f5745e0ec/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
