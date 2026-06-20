---
uid: Cuemon.Extensions.Net.StringExtensions
example:
- *content
---

`StringExtensions` in the `Net` namespace provides URL-encoding and URL-decoding extension methods on `string` with custom encoding support and null-safe handling. This example encodes `"hello world"` to `"hello+world"`, decodes it back, encodes with UTF-32 where each character produces 4 bytes, and handles query-string special characters in `"name=Jane Doe&city=Copenhagen"` producing `"name%3dJane+Doe%26city%3dCopenhagen"`. It also demonstrates null input safety where `((string)null).UrlEncode()` returns `null`. Console output confirms each round-trip operation and the null-safe result.

```csharp
using System.Text;
using System;
using Cuemon.Extensions.Net;
using Cuemon.Text;

using Cuemon;
namespace MyApp.Extensions.Net
{
    public class StringExtensionsExample
    {
        public void Demonstrate()
        {
            // URL-encode a string directly (no Decorator needed)
            string encoded = "hello world".UrlEncode();
            Console.WriteLine(encoded); // "hello+world"

            // URL-decode a previously encoded string
            string decoded = "hello+world".UrlDecode();
            Console.WriteLine(decoded); // "hello world"

            // Encode with a custom encoding
            string encodedUtf32 = "a & b".UrlEncode(o =>
            {
                o.Encoding = Encoding.UTF32;
            });

            // Encode query-string special characters
            string queryEncoded = "name=Jane Doe&city=Copenhagen".UrlEncode();
            Console.WriteLine(queryEncoded); // "name%3dJane+Doe%26city%3dCopenhagen"

            // Decode back
            string queryDecoded = queryEncoded.UrlDecode();
            Console.WriteLine(queryDecoded); // "name=Jane Doe&city=Copenhagen"

            // Handle null input safely
            string nullResult = ((string)null).UrlEncode();
            Console.WriteLine(nullResult == null); // True

}}
}

```
