---
uid: Cuemon.Xml.XmlStreamFactory
example:
- *content
---

The following example builds an XML document inline using `XmlStreamFactory.CreateStream` with an `XmlWriter` delegate. The resulting stream is read back as a string, producing a well-formed XML document with a `<Configuration>` root element.

```csharp
using System;
using System.IO;
using System.Xml;
using Cuemon.Xml;

namespace Cuemon.Xml;

public class XmlStreamFactoryExample
{
    public void Demonstrate()
    {
        using var stream = XmlStreamFactory.CreateStream(writer =>
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Configuration");
            writer.WriteElementString("AppName", "MyApp");
            writer.WriteElementString("Version", "1.0");
            writer.WriteEndElement();
            writer.WriteEndDocument();
        });

        using var reader = new StreamReader(stream);
        var xml = reader.ReadToEnd();
        Console.WriteLine(xml);
    }
}
```
