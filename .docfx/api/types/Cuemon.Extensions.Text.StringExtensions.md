---
uid: Cuemon.Extensions.Text.StringExtensions
example:
- *content
---

`StringExtensions` in the `Text` namespace provides encoding conversion between character encodings with custom fallback handling and ASCII sanitization. This example takes `"Café au lait: 2,50 €"` and converts it to UTF-8 (preserves all characters), ASCII with `EncoderReplacementFallback("?")` (replaces `é` and `€` with `?`), and Windows-1252 (preserves Western European characters). It also demonstrates `ToAsciiEncodedString` for quick ASCII sanitization that removes unsupported characters by default. Console output shows each encoded result, such as `"Caf? au lait: 2,50 ?"` for the ASCII replacement case and `"Cafe au lait: 2,50 "` for the quick ASCII sanitization.

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
