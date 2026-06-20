---
uid: Cuemon.Xml.Serialization.XmlConvert
example:
- *content
---

The following example configures default `XmlSerializerOptions` on `XmlConvert` with UTF-8 encoding and tab indentation, then reads back the defaults and prints the encoding name. This demonstrates how to override global XML serialization settings.

```csharp
using System;
using System.Text;
using System.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Xml.Serialization;

public class XmlConvertExample
{
    public void Demonstrate()
    {
        XmlConvert.DefaultSettings = () => new XmlSerializerOptions
        {
            Writer = new XmlWriterSettings { Encoding = Encoding.UTF8, IndentChars = Cuemon.Alphanumeric.Tab }
        };

        var settings = XmlConvert.DefaultSettings();
        Console.WriteLine($"Default encoding: {settings.Writer.Encoding.EncodingName}");
    }
}
```
