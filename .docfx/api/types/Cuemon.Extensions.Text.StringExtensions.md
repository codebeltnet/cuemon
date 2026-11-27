---
uid: Cuemon.Extensions.Text.StringExtensions
example:
- *content
---

`StringExtensions` in the `Text` namespace provides encoding conversion between character encodings with custom fallback handling and ASCII sanitization. This example takes `"Café au lait: 2,50 €"` and converts it to UTF-8 (preserves all characters), ASCII with `EncoderReplacementFallback("?")` (replaces `é` and `€` with `?`), and UTF-16 (`Encoding.Unicode`). It also demonstrates `ToAsciiEncodedString` for quick ASCII sanitization that removes unsupported characters by default. Console output shows each encoded result, such as `"Caf? au lait: 2,50 ?"` for the ASCII replacement case and `"Caf au lait: 2,50 "` for the quick ASCII sanitization.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Extensions.Text;
using Cuemon.Text;

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

// Convert to UTF-16 (Unicode)
var unicode = withSpecialChars.ToEncodedString(o =>
{
    o.TargetEncoding = Encoding.Unicode;
    o.EncoderFallback = new EncoderReplacementFallback("?");
});
Console.WriteLine(unicode);

// ToAsciiEncodedString - quick ASCII conversion
var asciiQuick = withSpecialChars.ToAsciiEncodedString();
Console.WriteLine(asciiQuick); // "Caf au lait: 2,50 "
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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="dfe56fad8969075b991d4616"><a href="~/run-actions/dfe56fad8969075b991d4616/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/dfe56fad8969075b991d4616/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
