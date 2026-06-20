---
uid: Cuemon.Xml.StreamDecoratorExtensions
example:
- *content
---

The following example demonstrates how to create an <see cref="XmlReader" /> from a stream and detect the XML encoding.

```csharp
using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon;
using Cuemon.Xml;

namespace MyApp.Examples;

public static class StreamDecoratorExtensionsExample
{
    public static void Demonstrate()
    {
        const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><child>hello</child></root>";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
        using XmlReader reader = Decorator.Enclose(stream).ToXmlReader();
        reader.MoveToContent();

        Console.WriteLine(reader.LocalName);

        using var detectStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
        var detected = Decorator.Enclose(detectStream).TryDetectXmlEncoding(out var encoding);

        Console.WriteLine(detected);
        Console.WriteLine(encoding.EncodingName);
    }
}

```
