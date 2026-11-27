---
uid: Cuemon.Extensions.Xml.StreamExtensions
example:
- *content
---

`StreamExtensions` in the `Xml` namespace provides stream-based XML operations including `XmlReader` creation, indented copying, encoding detection, and namespace removal. This example loads a namespace-prefixed XML string into a `MemoryStream` for four operations: `ToXmlReader` to parse element names, `CopyXmlStream` with `Indent = true` for pretty-printed XML output, `TryDetectXmlEncoding` to identify `UTF-8` encoding, and `RemoveXmlNamespaceDeclarations` to strip namespace declarations and prefixes from elements. Console output shows element names (`ns:root`, `ns:item`), the indented XML, the detected encoding name (`Unicode (UTF-8)`), and the namespace-cleaned XML (`<root><item>Value</item></root>`).

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
            const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ns:root xmlns:ns=\"https://example.com/ns\"><ns:item>Value</ns:item></ns:root>";

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
<div class="docfx-run-actions" data-example-id="a27286f4a618d3e13702388a"><a href="~/run-actions/a27286f4a618d3e13702388a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a27286f4a618d3e13702388a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
