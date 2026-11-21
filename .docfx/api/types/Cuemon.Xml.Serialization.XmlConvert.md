---
uid: Cuemon.Xml.Serialization.XmlConvert
example:
- *content
---

The following example configures default `XmlSerializerOptions` on `XmlConvert` with UTF-8 encoding and tab indentation, then reads back the defaults and prints the encoding name. This demonstrates how to override global XML serialization settings.

```csharp
// Program.cs
using System;
using System.Xml;
using System.Text;
using Cuemon.Xml.Serialization;
using XmlConvert = Cuemon.Xml.Serialization.XmlConvert;

XmlConvert.DefaultSettings = () => new XmlSerializerOptions
{
    Writer = new XmlWriterSettings { Encoding = Encoding.UTF8, IndentChars = "  " }
};

var settings = XmlConvert.DefaultSettings();
Console.WriteLine($"Default encoding: {settings.Writer.Encoding.EncodingName}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="66b4b87c418aa393b7809e88"><a href="~/run-actions/66b4b87c418aa393b7809e88/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/66b4b87c418aa393b7809e88/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
