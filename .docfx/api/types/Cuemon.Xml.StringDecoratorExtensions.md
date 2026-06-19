---
uid: Cuemon.Xml.StringDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the XML `StringDecoratorExtensions` to escape, unescape, and sanitize XML strings through the `IDecorator` interface.

```csharp
using System;
using Cuemon;
using Cuemon.Xml;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {

        // Escape XML characters in a string
        var raw = "<hello & 'world' \"test\">";
        var escaped = Decorator.Enclose(raw).EscapeXml();
        Console.WriteLine($"Escaped:   {escaped}");
        // Output: &lt;hello &amp; &apos;world&apos; &quot;test&quot;&gt;

        // Unescape back to original
        var unescaped = Decorator.Enclose(escaped).UnescapeXml();
        Console.WriteLine($"Unescaped: {unescaped}");
        // Output: <hello & 'world' "test">

        // Sanitize a string to be a valid XML element name
        var invalidName = "123order-details!.xml";
        var sanitizedName = Decorator.Enclose(invalidName).SanitizeXmlElementName();
        Console.WriteLine($"Sanitized element name: {sanitizedName}");
        // Output: order-details.xml

        // Sanitize XML element text (remove control characters)
        var textWithControlChars = "Hello\x0001\x0002World";
        var cleanText = Decorator.Enclose(textWithControlChars).SanitizeXmlElementText();
        Console.WriteLine($"Clean text: {cleanText}");
        // Output: HelloWorld

        // Sanitize with CDATA section rules (removes ]]> sequences)
        var cdataText = "Some data with ]]> embedded";
        var safeCdata = Decorator.Enclose(cdataText).SanitizeXmlElementText(cdataSection: true);
        Console.WriteLine($"Safe CDATA: {safeCdata}");
        // Output: Some data with  embedded

}
}

```
