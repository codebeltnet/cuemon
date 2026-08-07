---
uid: Cuemon.Extensions.StringExtensions
example:
- *content
---

`StringExtensions` provides a comprehensive set of extension methods for `string` covering trimming, casing, content inspection, encoding, parsing, and utility operations. This example applies `TrimAll` to remove whitespace, `ToCasing` with `LowerCase`, `UpperCase`, and `TitleCase` modes, and content checks like `IsEmailAddress`, `IsGuid`, `IsHex`, `IsNumeric`, and `IsBase64`. It also demonstrates encoding conversions (`ToByteArray`, `ToHexadecimal`, `FromBase64`, `FromUrlEncodedBase64`), enum parsing (`"Monday".ToEnum<DayOfWeek>()`), delimited-string splitting (`SplitDelimited` with quoted fields), and utility operations such as `Count`, `Difference`, `JsEscape`, `Chunk`, `PrefixWith`, `SuffixWith`, and `ToGuid`. Console output confirms transformations like `" Hello, World! "` trimmed to `"Hello,World!"`, `"hello".SuffixWith(" world")` producing `"hello world"`, and `"Monday".ToEnum<DayOfWeek>()` returning `DayOfWeek.Monday`.

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Text;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {
        var value = " Hello, World! ";
        var answer = "yes";
        var chunkSource = "abcdefgh";
        var plainText = "Hello";
        var encodedText = "SGVsbG8=";
        var binarySource = "1101";
        var dayName = "Monday";
        var timeSource = "42";
        var counted = "hello";
        var quoted = "hello's";
        var guidSource = "550e8400-e29b-41d4-a716-446655440000";
        var uriSource = "https://example.com";
        var sequence = new[] { "1", "2", "3" };
        var emptySource = string.Empty;

        // Trim all whitespace characters
        var trimmed = value.TrimAll(); // "Hello,World!"

        // Convert to different casing styles
        var lower = value.ToCasing(CasingMethod.LowerCase); // " hello, world! "
        var upper = value.ToCasing(CasingMethod.UpperCase); // " HELLO, WORLD! "
        var title = value.ToCasing(CasingMethod.TitleCase, new CultureInfo("en-US")); // " Hello, World! "

        // Check string characteristics
        bool isEmail = value.IsEmailAddress(); // false
        bool isGuid = value.IsGuid(); // false
        bool isHex = value.IsHex(); // false
        bool isNumeric = value.IsNumeric(); // false
        bool isBase64 = value.IsBase64(); // false

        // Substring operations
        var before = value.SubstringBefore(","); // " Hello"
        var after = value.SuffixWithForwardingSlash(); // " Hello, World! /"
        var prefixed = value.PrefixWith(">>"); // ">> Hello, World! "

        // Remove and replace
        var removed = value.RemoveAll(" ", "!"); // "Hello,World"
        var replaced = value.ReplaceAll("world", "Earth"); // " Hello, Earth! "

        // Contains checks
        bool hasHello = value.ContainsAny("Hello", "World"); // true
        bool hasAll = value.ContainsAll("Hello", "World"); // true
        bool hasChar = value.ContainsAny('o', 'x'); // true

        // Equality checks
        bool equalsAny = answer.EqualsAny("yes", "no"); // true
        bool equalsAnyIgnoreCase = answer.EqualsAny(StringComparison.OrdinalIgnoreCase, "YES", "NO"); // true

        // StartsWith
        bool starts = value.StartsWith(" Hello"); // true

        // Chunk
        IEnumerable<string> chunks = chunkSource.Chunk(3); // ["abc", "def", "gh"]
        var chunksDefault = chunkSource.Chunk(); // ["abc", "def", "gh"]

        // Encoding conversions
        byte[] bytes = plainText.ToByteArray(o => o.Encoding = Encoding.UTF8);
        string hex = plainText.ToHexadecimal();
        string fromHex = hex.FromHexadecimal();

        // Base64
        byte[] base64Bytes = encodedText.FromBase64();
        var urlB64 = encodedText.FromUrlEncodedBase64(); // "Hello"

        // Enum parsing
        var day = dayName.ToEnum<DayOfWeek>(); // DayOfWeek.Monday

        // TimeSpan from string
        var ts = timeSource.ToTimeSpan(TimeUnit.Minutes); // 00:42:00

        // Delimited string splitting
        string csv = "apple,\"orange, citrus\",banana";
        string[] parts = csv.SplitDelimited(); // ["apple", "orange, citrus", "banana"]

        // Validate a sequence of strings against a target type
        bool isIntegerSequence = sequence.IsSequenceOf<int>(); // true

        // Additional string utilities
        int charCount = counted.Count('l'); // 2
        string diff = counted.Difference("world"); // "world"
        var biDigits = binarySource.FromBinaryDigits(); // new byte[] { 13 }
        bool emptyCheck = emptySource.IsNullOrEmpty(); // true
        bool emptySequenceCheck = sequence.IsNullOrEmpty(); // false
        var whitespaceSource = "   ";
        bool whiteSpaceCheck = whitespaceSource.IsNullOrWhiteSpace(); // true
        string jsEsc = quoted.JsEscape(); // "hello\\u0027s"
        string jsUnesc = quoted.JsUnescape(); // "hello's"
        string suffixed = counted.SuffixWith(" world"); // "hello world"
        Guid asGuid = guidSource.ToGuid();
        Uri asUri = uriSource.ToUri();
        Console.WriteLine(isIntegerSequence);
    }
}

```
