---
uid: Cuemon.Xml.Serialization.DynamicXmlConverter
example:
- *content
---

The following example creates a `DynamicXmlConverter<int>` with custom read and write delegates. Writing the value 42 produces an XML fragment containing a `<Value>` element; the converter can also read it back from XML.

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
