---
uid: Cuemon.Extensions.Xml.XmlCopyOptions
example:
- *content
---

The following example demonstrates how to use <xref:Cuemon.Extensions.Xml.XmlCopyOptions> to configure <see cref="T:System.Xml.XmlWriter"/> settings when copying XML content.

```csharp
// Program.cs
using System;
using System.IO;
using System.Xml;
using Cuemon.Extensions.Xml;
using MyApp.Examples;

new XmlCopyOptionsExample().CopyXmlWithCustomSettings();

namespace MyApp.Examples
{
    public class XmlCopyOptionsExample
    {
        public void CopyXmlWithCustomSettings()
        {
            // Configure writer settings for indentation and encoding
            var copyOptions = new XmlCopyOptions
            {
                WriterSettings = settings =>
                {
                    settings.Indent = true;
                    settings.IndentChars = "  ";
                    settings.OmitXmlDeclaration = false;
                    settings.NewLineOnAttributes = false;
                }
            };

            // Use the options to configure an XmlWriter during a copy operation
            string input = @"<root><item id=""1"">text</item></root>";

            using (var reader = XmlReader.Create(new StringReader(input)))
            using (var writer = XmlWriter.Create(Console.Out, ConfigureWriter(copyOptions)))
            {
                writer.WriteNode(reader, false);
            }
            // Output:
            // <?xml version="1.0" encoding="..."?>
            // <root>
            //   <item id="1">text</item>
            // </root>
        }

        private static XmlWriterSettings ConfigureWriter(XmlCopyOptions options)
        {
            var settings = new XmlWriterSettings();
            options.WriterSettings?.Invoke(settings);
            return settings;
        }

        public void UseDefaults()
        {
            // Default options have null WriterSettings (no custom configuration)
            var options = new XmlCopyOptions();
            Console.WriteLine($"WriterSettings: {(options.WriterSettings == null ? "null (no customization)" : "configured")}");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7dd44f9075c0ee4520590b2e"><a href="~/run-actions/7dd44f9075c0ee4520590b2e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7dd44f9075c0ee4520590b2e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
