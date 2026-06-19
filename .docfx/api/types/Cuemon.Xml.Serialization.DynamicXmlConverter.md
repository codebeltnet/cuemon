---
uid: Cuemon.Xml.Serialization.DynamicXmlConverter
example:
- *content
---

```csharp
using System;
using System.IO;
using System.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Xml.Serialization;

public class DynamicXmlConverterExample
{
    public void Demonstrate()
    {
        var converter = DynamicXmlConverter.Create<int>(
            writer: (w, value, entity) =>
            {
                w.WriteElementString("Value", value.ToString());
            },
            reader: (r, type) =>
            {
                r.ReadToDescendant("Value");
                return int.Parse(r.ReadElementContentAsString());
            });

        using var sw = new StringWriter();
        using var writer = XmlWriter.Create(sw);
        converter.WriteXml(writer, 42);
        writer.Flush();
        Console.WriteLine(sw.ToString());
    }
}
```
