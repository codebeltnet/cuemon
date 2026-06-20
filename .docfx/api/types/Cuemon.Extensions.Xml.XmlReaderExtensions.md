---
uid: Cuemon.Extensions.Xml.XmlReaderExtensions
example:
- *content
---

`XmlReaderExtensions` provides extension methods for `XmlReader` including navigation, hierarchy building, chunked reading, and stream conversion. This example starts with an XML string containing `<root>` and two `<item>` elements, then demonstrates `MoveToFirstElement` to position at the first element, `ToHierarchy` to build a tree of element nodes with child names, `Chunk(1)` to stream only the first element as a new indented `XmlReader`, and `ToStream` to convert the entire reader content into a readable stream. Console output shows the root element name (`"root"`), a hierarchy description with child names, the outer XML of the first chunk (`<item id="1">First</item>`), and the full XML content.

```csharp
using System;
using System.IO;
using System.Linq;
using System.Xml;
using Cuemon.Extensions.Xml;

namespace DocExamples;

public static class XmlReaderExtensionsExample
{
    private const string XmlDocument = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><item id=\"1\">First</item><item id=\"2\">Second</item></root>";

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
```
