---
uid: Cuemon.Extensions.Xml.Serialization.Converters.XmlConverterExtensions
example:
- *content
---

The following example registers built-in and custom XML converters, then reuses the configured list through <xref:Cuemon.Xml.Serialization.XmlSerializer>.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Xml.Serialization.Converters;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;

namespace DocExamples;

public static class XmlConverterExtensionsExample
{
    public static void Demonstrate()
    {
        IList<XmlConverter> converters = new List<XmlConverter>();

        converters.InsertXmlConverter<string>(
            0,
            (writer, value, entity) => writer.WriteElementString(entity?.LocalName ?? "Value", value),
            (reader, objectType) => reader.ReadElementContentAsString(),
            objectType => objectType == typeof(string),
            new XmlQualifiedEntity("String"));

        converters.AddXmlConverter<int>(
            (writer, value, entity) => writer.WriteElementString(entity?.LocalName ?? "Value", value.ToString()),
            (reader, objectType) => reader.ReadElementContentAsInt(),
            objectType => objectType == typeof(int),
            new XmlQualifiedEntity("Int32"));

        converters.AddEnumerableConverter();
        converters.AddExceptionDescriptorConverter(options => options.SensitivityDetails = FaultSensitivityDetails.All);
        converters.AddUriConverter();
        converters.AddDateTimeConverter();
        converters.AddTimeSpanConverter();
        converters.AddStringConverter();
        converters.AddExceptionConverter(includeStackTrace: true, includeData: true);
        converters.AddFailureConverter();

        var writerConverter = converters.FirstOrDefaultWriterConverter(typeof(Uri));
        var readerConverter = converters.FirstOrDefaultReaderConverter(typeof(string));

        var serializerOptions = new XmlSerializerOptions();
        serializerOptions.Converters.Clear();
        foreach (var converter in converters)
        {
            serializerOptions.Converters.Add(converter);
        }

        var serializer = XmlSerializer.Create(serializerOptions);
        using var stream = serializer.Serialize(new Uri("https://example.com/feed.xml"), typeof(Uri));
        using var xml = new StreamReader(stream);

        Console.WriteLine(writerConverter is not null && readerConverter is not null);
        Console.WriteLine(xml.ReadToEnd());
    }
}
```
