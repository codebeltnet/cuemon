---
uid: Cuemon.Xml.XmlWriterDecoratorExtensions
example:
- *content
---

`XmlWriterDecoratorExtensions` provides extension methods on `Decorator.Enclose` for serializing objects directly to `XmlWriter` with root element handling, conditional encapsulation, and custom root naming. This example creates an `XmlWriter` targeting a `StringWriter` with indented settings, then uses `WriteXmlRootElement` with a custom delegate that writes a root element, serializes an anonymous `Person` object, and conditionally wraps notes in an encapsulating `<Notes>` element via `WriteEncapsulatingElementIfNotNull`. A second example shows `WriteObject` to serialize a `Version` with a custom `RootName = new XmlQualifiedEntity("Version")`. Console output displays both XML results with the correct element structure.

```csharp
using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {

        var writer = new StringWriter();
        var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8, OmitXmlDeclaration = false });

        var decorator = Decorator.Enclose(xmlWriter);

        // Write a root element and serialize an object
        var person = new { FirstName = "John", LastName = "Doe", Age = 30 };
        decorator.WriteXmlRootElement(person, (w, value, rootEntity) =>
        {
            decorator.WriteStartElement(rootEntity);                          // <Root>
            decorator.WriteObject(value, typeof(object));                      // serialized person content
            decorator.WriteEncapsulatingElementIfNotNull("notes", new XmlQualifiedEntity("Notes"), (w2, notes) =>
            {
                w2.WriteString(notes);                                        // <Notes>notes</Notes>
            });
        });

        xmlWriter.Flush();
        string xml = writer.ToString();
        Console.WriteLine(xml);

        // Write an object directly without root element handling
        var version = new Version(1, 0, 0, 0);
        var xmlWriter2 = XmlWriter.Create(new StringWriter(), new XmlWriterSettings { Indent = true });
        var decorator2 = Decorator.Enclose(xmlWriter2);
        decorator2.WriteObject(version, o =>
        {
            o.Settings.RootName = new XmlQualifiedEntity("Version");
        });
        xmlWriter2.Flush();
        // <Version>
        //   <Major>1</Major>
        //   <Minor>0</Minor>
        //   <Build>0</Build>
        //   <Revision>0</Revision>
        // </Version>

}
}

```
