---
uid: Cuemon.Xml.Serialization.Formatters.XmlFormatterOptions
example:
- *content
---

The following example demonstrates how to configure XmlFormatterOptions for custom media types, fault sensitivity details, and synchronization with XmlConvert settings.

```csharp
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Cuemon.Diagnostics;
using Cuemon.Xml.Serialization.Formatters;

namespace MyApp.Examples
{
    public class XmlFormatterOptionsExample
    {
        public void Demonstrate()
        {
            // Configure XmlFormatter with custom settings.
            var options = new XmlFormatterOptions
            {
                SynchronizeWithXmlConvert = true,
                SensitivityDetails = FaultSensitivityDetails.None
            };

            // Customize the supported media types.
            options.SupportedMediaTypes = new List<MediaTypeHeaderValue>
            {
                XmlFormatterOptions.DefaultMediaType,
                new MediaTypeHeaderValue("text/xml"),
                new MediaTypeHeaderValue("application/problem+xml")
            };

            Console.WriteLine($"Default media type: {XmlFormatterOptions.DefaultMediaType}");  // application/xml
            Console.WriteLine($"Supported types: {options.SupportedMediaTypes.Count}");         // 3
            Console.WriteLine($"Synchronize: {options.SynchronizeWithXmlConvert}");             // True

            // Validate before use.
            options.ValidateOptions();
            Console.WriteLine("Options are valid.");

            // Use with a formatter.
            var formatter = new XmlFormatter(options);
            Console.WriteLine($"Formatter created with {formatter.GetType().Name}.");

}}
}

```
