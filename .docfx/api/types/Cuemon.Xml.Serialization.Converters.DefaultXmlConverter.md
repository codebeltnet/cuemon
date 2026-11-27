---
uid: Cuemon.Xml.Serialization.Converters.DefaultXmlConverter
example:
- *content
---

The following example demonstrates how <xref:Cuemon.Xml.Serialization.Converters.DefaultXmlConverter> can serialize and deserialize a simple XML value.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;
using System.Xml;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3f6dedaa279ad5c1e62aabcc"><a href="~/run-actions/3f6dedaa279ad5c1e62aabcc/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3f6dedaa279ad5c1e62aabcc/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
