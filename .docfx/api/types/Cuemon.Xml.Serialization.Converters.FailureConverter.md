---
uid: Cuemon.Xml.Serialization.Converters.FailureConverter
example:
- *content
---

`FailureConverter` serializes `Failure` objects to XML via `XmlFormatter`, providing structured error output including exception type, source, and message. This example creates a `Failure` from an `InvalidOperationException("The requested resource was not found.")` with `Source = "MyApi"` and `FaultSensitivityDetails.None`, configures an `XmlFormatter` with the converter, and serializes to XML. The resulting XML output includes `<InvalidOperationException>` with `<Source>MyApi</Source>` and `<Message>The requested resource was not found.</Message>`, showing the default serialization format for failure objects.

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
