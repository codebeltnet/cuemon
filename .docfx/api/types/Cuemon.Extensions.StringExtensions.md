---
uid: Cuemon.Extensions.StringExtensions
example:
- *content
---

`StringExtensions` provides a comprehensive set of extension methods for `string` covering trimming, casing, content inspection, encoding, parsing, and utility operations. This example applies `TrimAll` to remove whitespace, `ToCasing` with `LowerCase`, `UpperCase`, and `TitleCase` modes, and content checks like `IsEmailAddress`, `IsGuid`, `IsHex`, `IsNumeric`, and `IsBase64`. It also demonstrates encoding conversions (`ToByteArray`, `ToHexadecimal`, `FromBase64`, `FromUrlEncodedBase64`), enum parsing (`"Monday".ToEnum<DayOfWeek>()`), delimited-string splitting (`SplitDelimited` with quoted fields), and utility operations such as `Count`, `Difference`, `JsEscape`, `Chunk`, `PrefixWith`, `SuffixWith`, and `ToGuid`. Console output confirms transformations like `" Hello, World! "` trimmed to `"Hello,World!"`, `"hello".SuffixWith(" world")` producing `"hello world"`, and `"Monday".ToEnum<DayOfWeek>()` returning `DayOfWeek.Monday`.

```csharp
using System.Text;
using System;
using System.Globalization;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Text;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {

        var value = " Hello, World! ";

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
        bool equalsAny = "yes".EqualsAny("yes", "no"); // true

        // StartsWith
        bool starts = value.StartsWith(" Hello"); // true

        // Chunk
        var chunks = "abcdefgh".Chunk(3); // ["abc", "def", "gh"]

        // Encoding conversions
        byte[] bytes = "Hello".ToByteArray(o => o.Encoding = Encoding.UTF8);
        string hex = "Hello".ToHexadecimal();
        string fromHex = hex.FromHexadecimal();

        // Base64
        byte[] base64Bytes = "SGVsbG8=".FromBase64();

        // Enum parsing
        var day = "Monday".ToEnum<DayOfWeek>(); // DayOfWeek.Monday

        // TimeSpan from string
        var ts = "42".ToTimeSpan(TimeUnit.Minutes); // 00:42:00

        // Delimited string splitting
        string csv = "apple,\"orange, citrus\",banana";
        string[] parts = csv.SplitDelimited(); // ["apple", "orange, citrus", "banana"]

        // Validate a sequence of strings against a target type
        bool isIntegerSequence = new[] { "1", "2", "3" }.IsSequenceOf<int>(); // true

        // Additional string utilities
        int charCount = "hello".Count('l'); // 2
        string diff = "hello".Difference("world"); // "world"
        var biDigits = "1101".FromBinaryDigits(); // new byte[] { 13 }
        var urlB64 = "SGVsbG8=".FromUrlEncodedBase64(); // "Hello"
        bool emptyCheck = "".IsNullOrEmpty(); // true
        bool whiteSpaceCheck = "   ".IsNullOrWhiteSpace(); // true
        string jsEsc = "hello's".JsEscape(); // "hello\\u0027s"
        string jsUnesc = "hello\\u0027s".JsUnescape(); // "hello's"
        string suffixed = "hello".SuffixWith(" world"); // "hello world"
        Guid asGuid = "550e8400-e29b-41d4-a716-446655440000".ToGuid();
        Uri asUri = "https://example.com".ToUri();
        Console.WriteLine(isIntegerSequence);
    }
}

```
