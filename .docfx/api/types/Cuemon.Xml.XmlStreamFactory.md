---
uid: Cuemon.Xml.XmlStreamFactory
example:
- *content
---

The following example builds an XML document inline using `XmlStreamFactory.CreateStream` with an `XmlWriter` delegate. The resulting stream is read back as a string, producing a well-formed XML document with a `<Configuration>` root element.

```csharp
// Program.cs
using System;
using System.IO;
using System.Xml;
using Cuemon.Xml;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c50aa604b9b7f3de4de12834"><a href="~/run-actions/c50aa604b9b7f3de4de12834/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c50aa604b9b7f3de4de12834/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
