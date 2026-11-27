---
uid: Cuemon.Xml.Serialization.XmlSerializerOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="XmlSerializerOptions" /> for custom root names and XML reader/writer settings.

```csharp
// Program.cs
using System;
using System.Xml;
using Cuemon.Xml.Serialization;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4ce00f82f63c1f0788e350da"><a href="~/run-actions/4ce00f82f63c1f0788e350da/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4ce00f82f63c1f0788e350da/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
