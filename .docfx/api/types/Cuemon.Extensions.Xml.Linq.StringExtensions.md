---
uid: Cuemon.Extensions.Xml.Linq.StringExtensions
example:
- *content
---

The following example demonstrates how to parse XML strings into <xref:System.Xml.Linq.XElement> using the <xref:Cuemon.Extensions.Xml.Linq.StringExtensions> class.

```csharp
using System;
using System.Xml.Linq;
using Cuemon.Extensions.Xml.Linq;

namespace MyApp.Examples;

public class StringExtensionsExample
{
    public void Demonstrate()
    {
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

}}
}

```
