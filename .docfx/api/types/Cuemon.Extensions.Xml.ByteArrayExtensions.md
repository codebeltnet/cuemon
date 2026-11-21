---
uid: Cuemon.Extensions.Xml.ByteArrayExtensions
example:
- *content
---

The following example demonstrates how to convert a byte array to an [XmlReader](https://learn.microsoft.com/dotnet/api/system.xml.xmlreader) using the <xref:Cuemon.Extensions.Xml.ByteArrayExtensions> class.

```csharp
// Program.cs
using System;
using System.Text;
using System.Xml;
using Cuemon.Extensions.Xml;

byte[] xmlData = Encoding.UTF8.GetBytes("<root><item>Value</item></root>");

// Convert the byte array to an XmlReader
using (XmlReader reader = xmlData.ToXmlReader())
{
    while (reader.Read())
    {
        if (reader.NodeType == XmlNodeType.Element)
        {
            Console.WriteLine(reader.Name);

}}}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="61a5acfbdf7181ef2fad373b"><a href="~/run-actions/61a5acfbdf7181ef2fad373b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/61a5acfbdf7181ef2fad373b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
