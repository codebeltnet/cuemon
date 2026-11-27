---
uid: Cuemon.Extensions.Xml.XmlReaderExtensions
example:
- *content
---

`XmlReaderExtensions` provides extension methods for `XmlReader` including navigation, hierarchy building, chunked reading, and stream conversion. This example starts with an XML string containing `<root>` and two `<item>` elements, then demonstrates `MoveToFirstElement` to position at the first element, `ToHierarchy` to build a tree of element nodes with child names, `Chunk(1)` to stream only the first element as a new indented `XmlReader`, and `ToStream` to convert the entire reader content into a readable stream. Console output shows the root element name (`"root"`), a hierarchy description with child names, the outer XML of the first chunk (`<root><item>First</item></root>` formatted with indentation), and the full XML content.

```csharp
// Program.cs
using System;
using System.IO;
using System.Linq;
using System.Xml;
using Cuemon.Extensions.Xml;
using DocExamples;

XmlReaderExtensionsExample.Demonstrate();

namespace DocExamples
{
    public static class XmlReaderExtensionsExample
    {
        private const string XmlDocument = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><item>First</item><item>Second</item></root>";

        public static void Demonstrate()
        {
            Console.WriteLine(GetRootName());
            Console.WriteLine(DescribeHierarchy());
            Console.WriteLine(ReadFirstChunk());
            Console.WriteLine(CopyXml());
        }

        private static string GetRootName()
        {
            using var xmlReader = XmlReader.Create(new StringReader(XmlDocument));
            return xmlReader.MoveToFirstElement() ? xmlReader.LocalName : string.Empty;
        }

        private static string DescribeHierarchy()
        {
            using var xmlReader = XmlReader.Create(new StringReader(XmlDocument));
            var hierarchy = xmlReader.ToHierarchy();
            var childNames = string.Join(", ", hierarchy.GetChildren().Select(child => child.Instance.Name));
            return $"{hierarchy.Instance.Name}: {childNames}";
        }

        private static string ReadFirstChunk()
        {
            using var xmlReader = XmlReader.Create(new StringReader(XmlDocument));
            using var firstChunk = xmlReader.Chunk(1, settings => settings.Indent = true).First();

            firstChunk.MoveToFirstElement();
            return firstChunk.ReadOuterXml();
        }

        private static string CopyXml()
        {
            using var xmlReader = XmlReader.Create(new StringReader(XmlDocument));
            using var xmlStream = xmlReader.ToStream();
            using var streamReader = new StreamReader(xmlStream);

            return streamReader.ReadToEnd();
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="275a28ae5bde827889dbdded"><a href="~/run-actions/275a28ae5bde827889dbdded/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/275a28ae5bde827889dbdded/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
