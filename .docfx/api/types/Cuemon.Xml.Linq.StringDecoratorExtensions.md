---
uid: Cuemon.Xml.Linq.StringDecoratorExtensions
example:
- *content
---

The following example demonstrates how to validate and parse XML strings into XElement objects using StringDecoratorExtensions, with support for load options and whitespace preservation.

```csharp
using System;
using System.Xml.Linq;
using Cuemon;
using Cuemon.Xml.Linq;

namespace MyApp.Xml
{
    public class StringDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // Valid XML string
            var validXml = "<root><item id=\"1\">value</item></root>";

            // Check if the string is valid XML
            var isValid = Decorator.Enclose(validXml).IsXmlString();
            Console.WriteLine($"Is valid XML: {isValid}"); // True

            // Try to parse the XML string into an XElement
            if (Decorator.Enclose(validXml).TryParseXElement(out var element))
            {
                Console.WriteLine($"Root element: {element.Name}");
                Console.WriteLine($"Inner XML: {element}");

                // Navigate the parsed XElement
                var item = element.Element("item");
                Console.WriteLine($"Item id attribute: {item?.Attribute("id")?.Value}");
                Console.WriteLine($"Item value: {item?.Value}");

            // Invalid XML string
            var invalidXml = "not xml at all";
            var isInvalid = Decorator.Enclose(invalidXml).IsXmlString();
            Console.WriteLine($"Is valid XML: {isInvalid}"); // False

            // TryParse will return false for invalid XML
            if (!Decorator.Enclose(invalidXml).TryParseXElement(out var badElement))
            {
                Console.WriteLine("Could not parse invalid XML.");
                Console.WriteLine($"Result is null: {badElement == null}"); // True

            // TryParse with load options
            var xmlWithWhitespace = "  <root><item>value</item></root>  ";
            if (Decorator.Enclose(xmlWithWhitespace)
                .TryParseXElement(LoadOptions.PreserveWhitespace, out var preserveElement))
            {
                Console.WriteLine($"Preserved whitespace element: '{preserveElement}'");

}}}}}
}

```
