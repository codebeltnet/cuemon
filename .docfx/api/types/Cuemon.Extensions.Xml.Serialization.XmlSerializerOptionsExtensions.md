---
uid: Cuemon.Extensions.Xml.Serialization.XmlSerializerOptionsExtensions
example:
- *content
---

The following example demonstrates how to apply custom <xref:Cuemon.Xml.Serialization.XmlSerializerOptions> to the default <see cref="System.Xml.XmlConvert"/> settings using the <xref:Cuemon.Extensions.Xml.Serialization.XmlSerializerOptionsExtensions> class.

```csharp
// Program.cs
using System;
using System.Text;
using System.Xml;
using Cuemon.Extensions.Xml.Serialization;
using Cuemon.Xml.Serialization;

// Configure XmlSerializerOptions
var options = new XmlSerializerOptions
{
    Reader = new XmlReaderSettings { IgnoreWhitespace = true },
    Writer = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }
};

// Apply to default XmlConvert settings
options.ApplyToDefaultSettings();

// After this call, XmlConvert.DefaultSettings will reflect the options
Console.WriteLine(options.Writer.Indent);
Console.WriteLine(options.Reader.IgnoreWhitespace);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6ad5e1d298f529d0ac598bc7"><a href="~/run-actions/6ad5e1d298f529d0ac598bc7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6ad5e1d298f529d0ac598bc7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
