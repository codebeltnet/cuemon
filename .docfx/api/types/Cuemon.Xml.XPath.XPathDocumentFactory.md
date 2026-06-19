---
uid: Cuemon.Xml.XPath.XPathDocumentFactory
example:
- *content
---

```csharp
using System;
using System.Xml.XPath;
using Cuemon.Xml.XPath;

namespace Cuemon.Xml.XPath;

public class XPathDocumentFactoryExample
{
    public void Demonstrate()
    {
        var xml = "<root><item id=\"1\">Hello</item></root>";
        var doc = XPathDocumentFactory.CreateDocument(xml);

        var navigator = doc.CreateNavigator();
        var value = navigator.SelectSingleNode("//item/text()")?.Value;
        Console.WriteLine($"XPath result: {value}");
    }
}
```
