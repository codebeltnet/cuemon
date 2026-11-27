---
uid: Cuemon.StringDecoratorExtensions
example:
- *content
---

`StringDecoratorExtensions` provides extension methods on `Decorator.Enclose` for string manipulation including casing conversion, encoding, URI conversion, set operations, and content inspection. This example wraps `" Hello World! "` and applies `ToCasing` with `LowerCase`, `UpperCase`, and `TitleCase` modes, converts the string to a byte array and stream with configurable encoding, and extracts the character-set difference between `"Hello World!"` and `"Hello Universe!"` using `Difference`. Key steps also include `StartsWith` checks with multiple candidate strings and `ContainsAny` for character matching. Console output confirms each transformed value, such as `" hello world! "` for lower-casing and `"Univs"` for the set difference.

```csharp
// Program.cs
using System;
using System.Globalization;
using System.IO;
using System.Text;
using Cuemon;
using Cuemon.Text;

string value = " Hello World! ";
var decorator = Decorator.Enclose(value);

// Change casing via decorator
string lower = decorator.ToCasing(CasingMethod.LowerCase);          // " hello world! "
string upper = decorator.ToCasing(CasingMethod.UpperCase);          // " HELLO WORLD! "
string title = decorator.ToCasing(CasingMethod.TitleCase, new CultureInfo("en-US")); // " Hello World! "

// Convert to byte array
byte[] bytes = decorator.ToByteArray(o => o.Encoding = Encoding.UTF8);

// Convert to stream
using Stream stream = decorator.ToStream(o => o.Encoding = Encoding.UTF8);

// Convert to URI
string url = "https://www.example.com";
var uriDecorator = Decorator.Enclose(url);
Uri uri = uriDecorator.ToUri(); // https://www.example.com/

// Encoding conversions
string ascii = decorator.ToAsciiEncodedString(o => o.Encoding = Encoding.UTF8); // " Hello World! " (non-ASCII chars replaced with empty)
string encoded = decorator.ToEncodedString(o =>
{
    o.TargetEncoding = Encoding.ASCII;
    o.EncoderFallback = new EncoderReplacementFallback("?");
});

// StartsWith checks
bool starts = decorator.StartsWith(" Hello");                     // True
bool startsIgnore = decorator.StartsWith(StringComparison.OrdinalIgnoreCase, "hello"); // False (leading space)
bool startsAny = decorator.StartsWith("Hi", "Hello");             // False (leading space)

// Set difference
var helloDecorator = Decorator.Enclose("Hello World!");
string diff = helloDecorator.Difference("Hello Universe!"); // "Univs"

// ContainsAny for characters
bool hasChar = decorator.ContainsAny('o', StringComparison.Ordinal);                          // true
bool hasChars = decorator.ContainsAny(StringComparison.Ordinal, 'x', 'y'); // false

Console.WriteLine(lower);
Console.WriteLine(upper);
Console.WriteLine(bytes.Length);
Console.WriteLine(uri);
Console.WriteLine(starts);
Console.WriteLine(startsIgnore);
Console.WriteLine(startsAny);
Console.WriteLine(diff);
Console.WriteLine(hasChar);
Console.WriteLine(hasChars);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e3ae5b35547d3ae5d8af85df"><a href="~/run-actions/e3ae5b35547d3ae5d8af85df/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e3ae5b35547d3ae5d8af85df/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
