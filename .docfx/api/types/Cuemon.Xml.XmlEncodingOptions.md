---
uid: Cuemon.Xml.XmlEncodingOptions
example:
- *content
---

The following example demonstrates how to configure XmlEncodingOptions to control character encoding and XML declaration behavior when writing XML documents.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Text;
using Cuemon.Xml;

// Configure XML encoding options.
var options = new XmlEncodingOptions
{
    Encoding = Encoding.UTF8,
    Preamble = EncodingOptions.DefaultPreambleSequence,
    OmitXmlDeclaration = false
};

Console.WriteLine($"Encoding:           {options.Encoding.WebName}");          // utf-8
Console.WriteLine($"Omit XML declaration: {options.OmitXmlDeclaration}");       // False

// Write XML with explicit encoding settings.
var settings = new XmlWriterSettings
{
    Encoding = options.Encoding,
    OmitXmlDeclaration = options.OmitXmlDeclaration,
    Indent = true
};

using var writer = XmlWriter.Create(Stream.Null, settings);
writer.WriteStartDocument();
writer.WriteStartElement("root");
writer.WriteElementString("value", "Hello, World!");
writer.WriteEndElement();
writer.WriteEndDocument();
writer.Flush();

// Omit the XML declaration.
options.OmitXmlDeclaration = true;
Console.WriteLine($"Omit XML declaration now: {options.OmitXmlDeclaration}");  // True
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3aac83bb28540493975983f4"><a href="~/run-actions/3aac83bb28540493975983f4/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3aac83bb28540493975983f4/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
