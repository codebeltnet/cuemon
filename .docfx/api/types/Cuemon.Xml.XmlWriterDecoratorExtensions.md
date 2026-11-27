---
uid: Cuemon.Xml.XmlWriterDecoratorExtensions
example:
- *content
---

`XmlWriterDecoratorExtensions` provides extension methods on `Decorator.Enclose` for serializing objects directly to `XmlWriter` with root element handling, conditional encapsulation, and custom root naming. This example creates an `XmlWriter` targeting a `StringWriter` with indented settings, then uses `WriteXmlRootElement` with a custom delegate that writes a root element, serializes an anonymous `Person` object, and also demonstrates `WriteStartElement` plus `WriteEncapsulatingElementIfNotNull` for a `<LastName>` element. A second example shows `WriteObject` to serialize a `Version` with a custom `RootName = new XmlQualifiedEntity("Version")`. Console output displays both XML results with the correct element structure.

```csharp
// Program.cs
using System;
using System.IO;
using System.Xml;
using Cuemon;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;

var writer = new StringWriter();
using var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });
var decorator = Decorator.Enclose(xmlWriter);

var person = new { FirstName = "John", LastName = "Doe", Age = 30 };
decorator.WriteXmlRootElement(person, (w, value, rootEntity) =>
{
    w.WriteElementString("FirstName", value.FirstName);
    w.WriteElementString("LastName", value.LastName);
    w.WriteElementString("Age", value.Age.ToString());
}, new XmlQualifiedEntity("Person"));

xmlWriter.Flush();
Console.WriteLine(writer.ToString());

var manualWriter = new StringWriter();
using var manualXmlWriter = XmlWriter.Create(manualWriter, new XmlWriterSettings { Indent = true });
var manualDecorator = Decorator.Enclose(manualXmlWriter);
manualDecorator.WriteStartElement(new XmlQualifiedEntity("Person"));
manualDecorator.WriteEncapsulatingElementIfNotNull("Doe", new XmlQualifiedEntity("LastName"), (w, value) => w.WriteString(value));
manualXmlWriter.WriteEndElement();
manualXmlWriter.Flush();
Console.WriteLine(manualWriter.ToString());

var versionWriter = new StringWriter();
using var versionXmlWriter = XmlWriter.Create(versionWriter, new XmlWriterSettings { Indent = true });
var versionDecorator = Decorator.Enclose(versionXmlWriter);
versionDecorator.WriteObject(new Version(1, 0, 0, 0), o => o.Settings.RootName = new XmlQualifiedEntity("Version"));
versionXmlWriter.Flush();
Console.WriteLine(versionWriter.ToString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7ee751fbf1a097b26a323aa2"><a href="~/run-actions/7ee751fbf1a097b26a323aa2/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7ee751fbf1a097b26a323aa2/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
