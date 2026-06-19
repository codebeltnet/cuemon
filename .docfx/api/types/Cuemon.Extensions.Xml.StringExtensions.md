---
uid: Cuemon.Extensions.Xml.StringExtensions
example:
- *content
---

The following example demonstrates how to escape and unescape XML special characters, sanitize strings for use as XML element names, and remove invalid XML control characters using StringExtensions.

```csharp
using System;
using Cuemon.Extensions.Xml;

namespace MyApp.Xml
{
    public class StringExtensionsExample
    {
        public void Demonstrate()
        {
            // Escape XML special characters
            var unsafeText = "Use & < > \" ' in XML";
            var escaped = unsafeText.EscapeXml();
            Console.WriteLine(escaped); // "Use &amp; &lt; &gt; &quot; &apos; in XML"

            // Unescape XML back to original text
            var unescaped = escaped.UnescapeXml();
            Console.WriteLine(unescaped); // "Use & < > \" ' in XML"

            // Sanitize a string for use as an XML element name
            var invalidName = "1st Element Name!";
            var sanitizedName = invalidName.SanitizeXmlElementName();
            Console.WriteLine(sanitizedName); // "_st_Element_Name_"

            // Sanitize XML element text (remove control characters except \t, \n, \r)
            var invalidText = "Valid text \x00 with \x01 control chars";
            var sanitizedText = invalidText.SanitizeXmlElementText();
            Console.WriteLine(sanitizedText); // "Valid text  with  control chars"

            // Sanitize XML element text with CDATA section rules (discourages "]]>" sequence)
            var cdataText = "text with ]]> inside";
            var sanitizedCdata = cdataText.SanitizeXmlElementText(cdataSection: true);
            Console.WriteLine(sanitizedCdata); // "text with ]] inside"

}}
}

```
