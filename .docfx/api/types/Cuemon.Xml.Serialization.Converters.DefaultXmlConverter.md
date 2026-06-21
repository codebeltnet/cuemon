---
uid: Cuemon.Xml.Serialization.Converters.DefaultXmlConverter
example:
- *content
---

The following example demonstrates how <xref:Cuemon.Xml.Serialization.Converters.DefaultXmlConverter> can serialize and deserialize a simple XML value.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;
using System.Xml;

namespace MyApp.Examples;

public static class DefaultXmlConverterExample
{
    public static void Demonstrate()
    {
        var converter = new DefaultXmlConverter(new XmlQualifiedEntity("String"), new List<XmlConverter>());

        using var buffer = new MemoryStream();
        using (var writer = XmlWriter.Create(buffer, new XmlWriterSettings { OmitXmlDeclaration = true }))
        {
            converter.WriteXml(writer, "Hello World");
        }

        buffer.Position = 0;
        using var reader = XmlReader.Create(buffer);
        var value = (string)converter.ReadXml(reader, typeof(string));

        Console.WriteLine(value);
        Console.WriteLine(converter.CanConvert(typeof(string)));
    }
}
```
