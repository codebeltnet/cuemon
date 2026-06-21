---
uid: Cuemon.Xml.Serialization.Converters.ExceptionConverter
example:
- *content
---

`ExceptionConverter` in the `Xml.Serialization` namespace serializes `Exception` instances to XML and deserializes them back, with optional stack trace and `Data` dictionary inclusion. This example creates an outer `InvalidOperationException` with an inner `ArgumentNullException` and a `"Server"` data entry, configures an `XmlFormatter` with the converter including stack trace and data, and serializes to XML output showing `<Source>`, `<Message>`, `<Stack>`, `<Data>`, and nested `<Inner>` elements. It also demonstrates deserializing from an XML string back to an exception instance with `converter.ReadXml`, and serialization without stack trace or data for simpler output. Console output displays the XML content and deserialized type name.

```csharp
using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;
using Cuemon.Xml.Serialization.Formatters;

namespace MyApp.Examples;

public class ExceptionConverterExample
{
    public void SerializeExceptionWithStackTrace()
    {
        // Create an exception with inner exception and custom data
        var inner = new ArgumentNullException("connectionString", "Value cannot be null.");
        var outer = new InvalidOperationException("Failed to connect to the database.", inner);
        outer.Data["Server"] = "db01.prod.example.com";

        // Configure the XML formatter with ExceptionConverter including stack trace and data
        var options = new XmlFormatterOptions();
        options.Settings.Converters.Add(new ExceptionConverter(includeStackTrace: true, includeData: true));

        // Serialize to XML
        var formatter = new XmlFormatter(options);
        using (var stream = formatter.Serialize(outer))
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            string xml = reader.ReadToEnd();
            Console.WriteLine(xml);
            // The output resembles:
            // <?xml version="1.0" encoding="utf-8"?>
            // <InvalidOperationException namespace="System">
            //   <Source>...</Source>
            //   <Message>Failed to connect to the database.</Message>
            //   <Stack>
            //     <Frame>at ExceptionConverterExample.SerializeExceptionWithStackTrace() ...</Frame>
            //   </Stack>
            //   <Data>
            //     <Server>db01.prod.example.com</Server>
            //   </Data>
            //   ...
            // </InvalidOperationException>
        }
    }

    public void DeserializeExceptionFromXml()
    {
        string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<InvalidOperationException>
  <Source>MyApp</Source>
  <Message>Something went wrong.</Message>
</InvalidOperationException>";

        var converter = new ExceptionConverter();
        using (var reader = XmlReader.Create(new StringReader(xml)))
        {
            var restored = converter.ReadXml(typeof(InvalidOperationException), reader);
            Console.WriteLine(restored.GetType().Name); // "InvalidOperationException"
            Console.WriteLine(restored.Message);        // "Something went wrong."
        }
    }

    public void SerializeWithoutStackTraceAndData()
    {
        var exception = new TimeoutException("The operation timed out.");
        var options = new XmlFormatterOptions();
        options.Settings.Converters.Add(new ExceptionConverter()); // defaults: false, false

        var formatter = new XmlFormatter(options);
        using (var stream = formatter.Serialize(exception))
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            string xml = reader.ReadToEnd();
            // Stack trace and Data are excluded from the output
            Console.WriteLine(xml);
        }
    }
}
```
