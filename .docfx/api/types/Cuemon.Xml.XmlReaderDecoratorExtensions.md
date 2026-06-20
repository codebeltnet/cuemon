---
uid: Cuemon.Xml.XmlReaderDecoratorExtensions
example:
- *content
---

The following example demonstrates how to move to the first XML element, chunk a document, and build a hierarchy from an <see cref="XmlReader" />.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Cuemon;
using Cuemon.Xml;

namespace MyApp.Examples;

public static class XmlReaderDecoratorExtensionsExample
{
    public static void Demonstrate()
    {
        const string xml = "<items><item id=\"1\"/><item id=\"2\"/><item id=\"3\"/></items>";

        using var reader = XmlReader.Create(new StringReader(xml));
        var hasElement = Decorator.Enclose(reader).MoveToFirstElement();

        Console.WriteLine(hasElement);
        Console.WriteLine(reader.LocalName);

        using var chunkReader = XmlReader.Create(new StringReader(xml));
        var chunks = new List<XmlReader>(Decorator.Enclose(chunkReader).Chunk(size: 2));

        Console.WriteLine(chunks.Count);

        using var hierarchyReader = XmlReader.Create(new StringReader(xml));
        var hierarchy = Decorator.Enclose(hierarchyReader).ToHierarchy();

        Console.WriteLine(hierarchy.Instance.Name);
    }
}
```
