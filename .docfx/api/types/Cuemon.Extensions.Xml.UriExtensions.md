---
uid: Cuemon.Extensions.Xml.UriExtensions
example:
- *content
---

`UriExtensions` in the `Xml` namespace creates `XmlReader` instances from file URIs with configurable reader settings. This example writes a simple XML snippet to a temporary file, constructs a `Uri` pointing to it, and calls `ToXmlReader` with settings that ignore XML comments and disable DTD processing. After positioning the reader with `MoveToFirstElement`, the local element name is output as `"root"`. A `finally` block ensures the temporary file is deleted after the demonstration.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Extensions.Xml;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="cff308a6e84282fbce049084"><a href="~/run-actions/cff308a6e84282fbce049084/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/cff308a6e84282fbce049084/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
