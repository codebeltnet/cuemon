---
uid: Cuemon.Extensions.Xml.Linq.StringExtensions
example:
- *content
---

The following example demonstrates how to parse XML strings into [XElement](https://learn.microsoft.com/dotnet/api/system.xml.linq.xelement) using the <xref:Cuemon.Extensions.Xml.Linq.StringExtensions> class.

```csharp
// Program.cs
using System;
using System.Xml.Linq;
using Cuemon.Extensions.Xml.Linq;

string xml = "<root><item id=\"1\">Value</item></root>";

// Try to parse the XML string into an XElement
if (xml.TryParseXElement(out XElement element))
{
    Console.WriteLine(element.Name); // root
    Console.WriteLine(element.Element("item")?.Value); // Value

// Check if a string is valid XML
bool isValid = xml.IsXmlString();
Console.WriteLine(isValid); // True

// Invalid XML returns false
string invalid = "<not-valid>";
bool isInvalid = invalid.IsXmlString();
Console.WriteLine(isInvalid); // False

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e4505be4d29a05cf0dff5dd3"><a href="~/run-actions/e4505be4d29a05cf0dff5dd3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e4505be4d29a05cf0dff5dd3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
