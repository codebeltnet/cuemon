---
uid: Cuemon.Xml.Serialization.DynamicXmlConverterCore
example:
- *content
---

`DynamicXmlConverterCore` enables creating custom XML converters using read and write delegates, supporting both read/write and write-only scenarios. This example creates a `Person` class and builds a `DynamicXmlConverter` with a writer delegate that serializes `FirstName`, `LastName`, and `Age` as child elements and a reader delegate that deserializes them back. A `Person` instance is serialized to XML and deserialized to confirm round-trip fidelity, and a write-only converter is also demonstrated for scenarios where only serialization is needed. Console output displays the serialized XML, deserialized property values, and the converter's `CanRead`/`CanWrite` capabilities.

```csharp
using System;
using System.IO;
using System.Xml;
using Cuemon.Xml.Serialization;

namespace MyApp.Xml
{
    public static class DynamicXmlConverterCoreExamples
    {
        public static void Demonstrate()
        {
            // Use the DynamicXmlConverter factory to create a converter for a custom type.
            // This converter knows how to read and write instances of Person to/from XML.
            DynamicXmlConverterCore converter = (DynamicXmlConverterCore)DynamicXmlConverter.Create<Person>(
                writer: (xmlWriter, person, element) =>
                {
                    xmlWriter.WriteStartElement(element?.LocalName ?? "Person");
                    xmlWriter.WriteElementString("FirstName", person.FirstName);
                    xmlWriter.WriteElementString("LastName", person.LastName);
                    xmlWriter.WriteElementString("Age", person.Age.ToString());
                    xmlWriter.WriteEndElement();
                },
                reader: (xmlReader, type) =>
                {
                    var person = new Person();
                    xmlReader.ReadStartElement();
                    person.FirstName = xmlReader.ReadElementContentAsString("FirstName", "");
                    person.LastName = xmlReader.ReadElementContentAsString("LastName", "");
                    person.Age = xmlReader.ReadElementContentAsInt("Age", "");
                    xmlReader.ReadEndElement();
                    return person;
                });

            converter.RootName = new XmlQualifiedEntity("Person");

            // The returned converter is an XmlConverter instance.
            Console.WriteLine("Can convert Person: {0}", converter.CanConvert(typeof(Person)));
            Console.WriteLine("Can read: {0}", converter.CanRead);   // true
            Console.WriteLine("Can write: {0}", converter.CanWrite); // true

            // Write a Person instance to XML.
            var person = new Person { FirstName = "John", LastName = "Doe", Age = 30 };
            var xml = new StringWriter();
            using (var xmlWriter = XmlWriter.Create(xml, new XmlWriterSettings { Indent = true }))
            {
                converter.WriteXml(xmlWriter, person);
            }
            Console.WriteLine(xml.ToString());

            // Read the Person back from XML.
            using (var xmlReader = XmlReader.Create(new StringReader(xml.ToString())))
            {
                var deserialized = (Person)converter.ReadXml(xmlReader, typeof(Person));
                Console.WriteLine("Deserialized: {0} {1}, Age {2}",
                    deserialized.FirstName, deserialized.LastName, deserialized.Age);
            }
        }

        public static void DemonstrateWriteOnly()
        {
            // Create a write-only converter (no reader delegate).
            var writeOnly = DynamicXmlConverter.Create<Person>(
                writer: (w, p, e) =>
                {
                    w.WriteStartElement(e?.LocalName ?? "Person");
                    w.WriteAttributeString("name", p.FirstName + " " + p.LastName);
                    w.WriteEndElement();
                });

            Console.WriteLine("Can read: {0}", writeOnly.CanRead);  // false
            Console.WriteLine("Can write: {0}", writeOnly.CanWrite); // true

            var person = new Person { FirstName = "Jane", LastName = "Smith", Age = 25 };
            var xml = new StringWriter();
            using (var xmlWriter = XmlWriter.Create(xml))
            {
                writeOnly.WriteXml(xmlWriter, person);
            }
            Console.WriteLine(xml.ToString());
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
```
