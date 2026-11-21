---
uid: Cuemon.Xml.Serialization.XmlSerializerOptionsDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the `ApplyToDefaultSettings` extension method to apply `XmlSerializerOptions` to the global `XmlConvert.DefaultSettings`.

```csharp
using System;
using System.Xml;
using Cuemon;
using Cuemon.Xml.Serialization;

namespace MyApp.Examples;

public class XmlSerializerOptionsDecoratorExtensionsExample
{
    public static void Main()
    {
        var options = new XmlSerializerOptions
        {
            Writer = new XmlWriterSettings { Indent = true, IndentChars = "  " },
            RootName = new XmlQualifiedEntity("CustomRoot")
        };

        // Apply the options as the default XmlWriterSettings globally.
        Decorator.Enclose(options).ApplyToDefaultSettings();

        // When running XmlConvert.EncodeName or similar, the default settings apply.
        Console.WriteLine("Default settings applied successfully.");

}
}

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7842d58b92f3cbdc3abfd2a0"><a href="~/run-actions/7842d58b92f3cbdc3abfd2a0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7842d58b92f3cbdc3abfd2a0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
