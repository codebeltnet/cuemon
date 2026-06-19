---
uid: Cuemon.Extensions.Text.StringExtensions
example:
- *content
---

The following example demonstrates how to encode strings between different character encodings using StringExtensions, with support for fallback handling and ASCII sanitization.

```csharp
using System;
using System.Text;
using Cuemon.Extensions.Text;
using Cuemon.Text;

namespace MyApp.Text
{
    public class StringExtensionsExample
    {
        public void Demonstrate()
        {
            // ToEncodedString - encodes a string to a different encoding with fallback handling
            var withSpecialChars = "Café au lait: 2,50 €";

            // Default: uses ExceptionFallback, will throw for unrepresentable chars
            var encoded = withSpecialChars.ToEncodedString(o =>
            {
                o.TargetEncoding = Encoding.UTF8;
            });
            Console.WriteLine(encoded); // "Café au lait: 2,50 €"

            // Convert to ASCII with replacement fallback
            var asciiResult = withSpecialChars.ToEncodedString(o =>
            {
                o.TargetEncoding = Encoding.ASCII;
                o.EncoderFallback = new EncoderReplacementFallback("?");
            });
            Console.WriteLine(asciiResult); // "Caf? au lait: 2,50 ?"

            // Convert to Windows-1252 (Western European)
            var win1252 = withSpecialChars.ToEncodedString(o =>
            {
                o.TargetEncoding = Encoding.GetEncoding(1252);
                o.EncoderFallback = new EncoderReplacementFallback("?");
            });
            Console.WriteLine(win1252); // preserves most of the special chars

            // ToAsciiEncodedString - quick ASCII conversion
            var asciiQuick = withSpecialChars.ToAsciiEncodedString();
            Console.WriteLine(asciiQuick); // "Cafe au lait: 2,50 "
            // Uses EncoderReplacementFallback("") by default, so unsupported chars are removed silently

            // ToAsciiEncodedString with custom encoding options
            var asciiCustom = withSpecialChars.ToAsciiEncodedString(o =>
            {
                o.Preamble = PreambleSequence.Remove;
                o.Encoding = Encoding.UTF8;
            });
            Console.WriteLine(asciiCustom); // same result, no BOM

            // Practical example: sanitize user input to safe ASCII
            var userInput = "Hello  World — 2025 ©";
            var sanitized = userInput.ToAsciiEncodedString();
            Console.WriteLine(sanitized); // "Hello  World  2025 " (em dash and copyright removed)

}}
}

```
