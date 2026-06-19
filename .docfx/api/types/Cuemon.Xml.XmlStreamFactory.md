---
uid: Cuemon.Xml.XmlStreamFactory
example:
- *content
---

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
