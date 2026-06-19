---
uid: Cuemon.Xml.XmlDocumentFactory
example:
- *content
---

```csharp
using System;
using System.Xml;
using Cuemon.Xml;

namespace Cuemon.Xml;

public class XmlDocumentFactoryExample
{
    public void Demonstrate()
    {
        var xml = "<?xml version=\"1.0\"?><books><book title=\"1984\" author=\"Orwell\"/></books>";
        var doc = XmlDocumentFactory.CreateDocument(xml);

        var root = doc.DocumentElement;
        Console.WriteLine($"Root element: {root?.Name}");

        var book = root?.SelectSingleNode("book");
        Console.WriteLine($"Title: {book?.Attributes?["title"]?.Value}");
    }
}
```
