---
uid: Cuemon.Extensions.Xml.Serialization.XmlSerializerOptionsExtensions
example:
- *content
---

The following example demonstrates how to apply custom <xref:Cuemon.Xml.Serialization.XmlSerializerOptions> to the default <see cref="System.Xml.XmlConvert"/> settings using the <xref:Cuemon.Extensions.Xml.Serialization.XmlSerializerOptionsExtensions> class.

```csharp
using System;
using System.Text;
using System.Xml;
using Cuemon.Extensions.Xml.Serialization;
using Cuemon.Xml.Serialization;

namespace MyApp.Examples;

public class XmlSerializerOptionsExtensionsExample
{
    public void Demonstrate()
    {
        // Configure XmlSerializerOptions
        var options = new XmlSerializerOptions
        {
            Reader = new XmlReaderSettings { IgnoreWhitespace = true },
            Writer = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }
        };

        // Apply to default XmlConvert settings
        options.ApplyToDefaultSettings();

        // After this call, XmlConvert.DefaultSettings will reflect the options

}
}

```
