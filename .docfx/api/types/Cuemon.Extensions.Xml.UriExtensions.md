---
uid: Cuemon.Extensions.Xml.UriExtensions
example:
- *content
---

The following example demonstrates how to create an XmlReader from a file URI using UriExtensions, with configurable reader settings for comment handling and DTD processing.

```csharp
using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Extensions.Xml;

namespace DocExamples;

public static class UriExtensionsExample
{
    public static void Demonstrate()
    {
        var xml = "<?xml version=\"1.0\"?><root><!--comment--><item>42</item></root>";
        var filePath = Path.Combine(AppContext.BaseDirectory, "settings.xml");
        File.WriteAllText(filePath, xml, Encoding.UTF8);

        try
        {
            using var reader = new Uri(filePath).ToXmlReader(settings =>
            {
                settings.IgnoreComments = true;
                settings.DtdProcessing = DtdProcessing.Ignore;
            });

            if (reader.MoveToFirstElement())
            {
                Console.WriteLine(reader.LocalName);
            }
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
```
