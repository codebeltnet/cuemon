---
uid: Cuemon.Xml.XmlWriterDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Xml.XmlWriterDecoratorExtensions> to serialize objects directly to an <see cref="T:System.Xml.XmlWriter"/> via the decorator pattern.

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
