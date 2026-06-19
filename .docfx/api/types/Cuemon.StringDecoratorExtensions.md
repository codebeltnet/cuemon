---
uid: Cuemon.StringDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.StringDecoratorExtensions> to manipulate strings via the decorator pattern.

```csharp
using System;
using System.Globalization;
using System.IO;
using System.Text;
using Cuemon;
using Cuemon.Text;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {

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
        bool starts = decorator.StartsWith(" Hello");                     // true
        bool startsIgnore = decorator.StartsWith(StringComparison.OrdinalIgnoreCase, "hello"); // true
        bool startsAny = decorator.StartsWith("Hi", "Hello");             // true

        // Set difference
        var helloDecorator = Decorator.Enclose("Hello World!");
        string diff = helloDecorator.Difference("Hello Universe!"); // "Universe!"

        // ContainsAny for characters
        bool hasChar = decorator.ContainsAny('o', StringComparison.Ordinal);                          // true
        bool hasChars = decorator.ContainsAny(StringComparison.Ordinal, 'x', 'y'); // false
    }
}
```
