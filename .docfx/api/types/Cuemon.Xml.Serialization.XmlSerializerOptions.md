---
uid: Cuemon.Xml.Serialization.XmlSerializerOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="XmlSerializerOptions" /> for custom root names and XML reader/writer settings.

```csharp
using System;
using System.Xml;
using Cuemon.Xml.Serialization;

namespace MyApp.Examples;

public static class XmlSerializerOptionsExample
{
    public static void Demonstrate()
    {
        var options = new XmlSerializerOptions
        {
            RootName = new XmlQualifiedEntity("Invoice"),
            FlattenCollectionItems = true,
            Reader = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore },
            Writer = new XmlWriterSettings { Indent = true }
        };

        Console.WriteLine(options.RootName.LocalName);
        Console.WriteLine(options.FlattenCollectionItems);
        Console.WriteLine(options.Writer.Indent);
        Console.WriteLine(options.Converters.Count);
    }
}
```
