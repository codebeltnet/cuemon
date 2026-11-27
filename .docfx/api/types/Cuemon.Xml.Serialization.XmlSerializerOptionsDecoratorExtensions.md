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

        // Apply the options as the default XmlSerializerOptions globally.
        Decorator.Enclose(options).ApplyToDefaultSettings();

        // Read back the global default settings to observe the applied options.
        var defaultSettings = Cuemon.Xml.Serialization.XmlConvert.DefaultSettings();
        Console.WriteLine($"Default RootName: {defaultSettings.RootName.LocalName}");
        Console.WriteLine($"Default Indent: {defaultSettings.Writer.Indent}");
    }
}

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="801d6b47452104a9e048044f"><a href="~/run-actions/801d6b47452104a9e048044f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/801d6b47452104a9e048044f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
