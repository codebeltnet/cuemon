---
uid: Cuemon.Xml.Serialization.DynamicXmlSerializable
example:
- *content
---

The following example wraps an anonymous object with `DynamicXmlSerializable` and provides a custom writer that emits `Name` and `Score` child elements. The resulting XML is written to a `StringWriter` and printed to the console.

```csharp
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Cuemon.Xml.Serialization;

namespace Cuemon.Xml.Serialization;

public class DynamicXmlSerializableExample
{
    public void Demonstrate()
    {
        var data = new { Name = "Alice", Score = 95 };
        var serializable = DynamicXmlSerializable.Create(data,
            writer: (w, src) =>
            {
                w.WriteElementString("Name", src.Name);
                w.WriteElementString("Score", src.Score.ToString());
            });

        using var sw = new StringWriter();
        using var writer = XmlWriter.Create(sw);
        serializable.WriteXml(writer);
        writer.Flush();
        Console.WriteLine(sw.ToString());
    }
}
```
