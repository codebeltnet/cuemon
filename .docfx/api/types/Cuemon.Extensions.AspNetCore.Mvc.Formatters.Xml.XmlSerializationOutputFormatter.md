---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.XmlSerializationOutputFormatter
example:
- *content
---

The following example demonstrates how to construct an <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.XmlSerializationOutputFormatter"/> and inspect its supported media types and encodings.

```csharp
using System;
using System.Linq;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Cuemon.Xml.Serialization.Formatters;

namespace DocfxExamples;

public class XmlSerializationOutputFormatterExample
{
    public void Demonstrate()
    {
        var options = new XmlFormatterOptions();
        var formatter = new XmlSerializationOutputFormatter(options);

        Console.WriteLine($"Supported media types: {string.Join(", ", formatter.SupportedMediaTypes)}");
        Console.WriteLine($"Supported encodings: {string.Join(", ", formatter.SupportedEncodings.Select(e => e.WebName))}");

}
}

```
