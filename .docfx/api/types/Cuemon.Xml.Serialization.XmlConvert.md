---
uid: Cuemon.Xml.Serialization.XmlConvert
example:
- *content
---

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
