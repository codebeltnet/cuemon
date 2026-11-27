---
uid: Cuemon.Xml.Serialization.Converters.XmlConverterDecoratorExtensions
example:
- *content
---

The following example demonstrates how to register and query XML converters with <xref:Cuemon.Xml.Serialization.Converters.XmlConverterDecoratorExtensions>.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon;
using Cuemon.Diagnostics;
using Cuemon.Xml.Serialization.Converters;

var converters = new List<XmlConverter>();
var decorator = Decorator.Enclose((IList<XmlConverter>)converters);

decorator.AddDateTimeConverter();
decorator.AddTimeSpanConverter();
decorator.AddStringConverter();
decorator.AddUriConverter();
decorator.AddEnumerableConverter();
decorator.AddExceptionConverter(false, false);
decorator.AddFailureConverter();
decorator.AddExceptionDescriptorConverter(options => options.SensitivityDetails = FaultSensitivityDetails.All);
decorator.AddXmlConverter<Version>(
    (writer, version, qe) =>
    {
        writer.WriteStartElement(qe?.LocalName ?? "Version");
        writer.WriteString(version.ToString());
        writer.WriteEndElement();
    },
    (reader, type) => Version.Parse(reader.ReadElementContentAsString())
);
decorator.InsertXmlConverter<Uri>(0);

var writerConverter = decorator.FirstOrDefaultWriterConverter(typeof(Version));
var readerConverter = decorator.FirstOrDefaultReaderConverter(typeof(Uri));

Console.WriteLine(writerConverter != null);
Console.WriteLine(readerConverter != null);
Console.WriteLine(converters.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b2f4d8efa4d34ec62871001f"><a href="~/run-actions/b2f4d8efa4d34ec62871001f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b2f4d8efa4d34ec62871001f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
