---
uid: Cuemon.Xml.Linq.StringDecoratorExtensions
example:
- *content
---

`StringDecoratorExtensions` in the `Xml.Linq` namespace provides extension methods on `Decorator.Enclose` for validating and parsing XML strings into `XElement` objects. This example checks valid XML (`"<root><item id=\"1\">value</item></root>"`) with `IsXmlString` returning `True`, parses it via `TryParseXElement` and navigates to read the root name, item attribute, and value. Invalid XML (`"not xml at all"`) returns `False` for `IsXmlString` and `TryParseXElement` returns `false` with `null`. It also demonstrates `TryParseXElement` with `LoadOptions.PreserveWhitespace` to retain whitespace in the parsed output. Console output confirms each validation and parsing result.

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
