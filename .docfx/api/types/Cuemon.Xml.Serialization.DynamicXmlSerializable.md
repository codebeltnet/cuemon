---
uid: Cuemon.Xml.Serialization.DynamicXmlSerializable
example:
- *content
---

The following example wraps an anonymous object with `DynamicXmlSerializable` and provides a custom writer that emits `Name` and `Score` child elements. The resulting XML is written to a `StringWriter` and printed to the console.

```csharp
// Program.cs
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Cuemon.Xml.Serialization;

var data = new { Name = "Alice", Score = 95 };
var serializable = DynamicXmlSerializable.Create(data,
    writer: (w, src) =>
    {
        w.WriteStartElement("Person");
        w.WriteElementString("Name", src.Name);
        w.WriteElementString("Score", src.Score.ToString());
        w.WriteEndElement();
    });

using var sw = new StringWriter();
using var writer = XmlWriter.Create(sw);
serializable.WriteXml(writer);
writer.Flush();
Console.WriteLine(sw.ToString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c8ea7d5ef35238756d393931"><a href="~/run-actions/c8ea7d5ef35238756d393931/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c8ea7d5ef35238756d393931/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
