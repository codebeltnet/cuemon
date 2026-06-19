---
uid: Cuemon.Extensions.Net.StringExtensions
example:
- *content
---

The following example demonstrates how to URL-encode and URL-decode strings directly using StringExtensions, with support for custom encodings and null-safe handling.

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
