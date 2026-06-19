---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.XmlSerializationInputFormatter
example:
- *content
---

The following example demonstrates how to construct an <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.XmlSerializationInputFormatter"/> and inspect its supported media types and encodings.

```csharp
using System;
using System.Linq;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Cuemon.Xml.Serialization.Formatters;

namespace DocfxExamples;

public class XmlSerializationInputFormatterExample
{
    public void Demonstrate()
    {
        var options = new XmlFormatterOptions();
        var formatter = new XmlSerializationInputFormatter(options);

        Console.WriteLine($"Supported media types: {string.Join(", ", formatter.SupportedMediaTypes)}");
        Console.WriteLine($"Supported encodings: {string.Join(", ", formatter.SupportedEncodings.Select(e => e.WebName))}");

}
}

```
