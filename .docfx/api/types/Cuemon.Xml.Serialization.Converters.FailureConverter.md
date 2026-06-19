---
uid: Cuemon.Xml.Serialization.Converters.FailureConverter
example:
- *content
---

The following example demonstrates how to serialize a <xref:Cuemon.Diagnostics.Failure> object to XML using the <xref:Cuemon.Xml.Serialization.Converters.FailureConverter>.

```csharp
using System;
using System.IO;
using Cuemon.Diagnostics;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;
using Cuemon.Xml.Serialization.Formatters;

namespace MyApp.Examples;

public class FailureConverterExample
{
    public void SerializeFailureToXml()
    {
        // Create a Failure from an exception
        var exception = new InvalidOperationException("The requested resource was not found.")
        {
            Source = "MyApi"
        };
        var failure = new Failure(exception, FaultSensitivityDetails.None);

        // Configure the XML formatter with the FailureConverter
        var options = new XmlFormatterOptions();
        options.Settings.Converters.Add(new FailureConverter());

        // Serialize to XML
        var formatter = new XmlFormatter(options);
        using (var stream = formatter.Serialize(failure))
        using (var reader = new StreamReader(stream))
        {
            string xml = reader.ReadToEnd();
            Console.WriteLine(xml);
        // The output resembles:
        // <?xml version="1.0" encoding="utf-8"?>
        // <InvalidOperationException namespace="System">
        //   <Source>MyApi</Source>
        //   <Message>The requested resource was not found.</Message>
        // </InvalidOperationException>

}}
}

```
