---
uid: Cuemon.Net.ByteArrayDecoratorExtensions
example:
- *content
---

`ByteArrayDecoratorExtensions` in the `Net` namespace provides URL-encoding extension methods on `Decorator.Enclose` for byte arrays with partial encoding and custom encoding support. This example creates UTF-8 bytes from `"hello world & more <stuff>"` and calls `UrlEncode` with default parameters producing `"hello+world+%26+more+%3cstuff%3e"`, then demonstrates partial encoding on the first 5 bytes of `"a & b & c"` producing `"a+%26+b"`. It also shows custom UTF-32 encoding and empty array handling where `Array.Empty<byte>()` returns an encoded array of length `0`. Console output displays each encoded result.

```csharp
using System;
using System.Text;
using Cuemon;
using Cuemon.Net;
using Cuemon.Text;

namespace MyApp.Net
{
    public class ByteArrayDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // Create bytes containing characters that need URL encoding
            byte[] data = Encoding.UTF8.GetBytes("hello world & more <stuff>");

            // URL-encode the bytes with default position (0) and length (all)
            byte[] encoded = Decorator.Enclose(data).UrlEncode();
            string encodedString = Encoding.ASCII.GetString(encoded);
            Console.WriteLine(encodedString); // "hello+world+%26+more+%3cstuff%3e"

            // Encode only a portion of the byte array
            byte[] partial = Encoding.UTF8.GetBytes("a & b & c");
            byte[] encodedPartial = Decorator.Enclose(partial).UrlEncode(position: 0, bytesToRead: 5);
            string partialString = Encoding.ASCII.GetString(encodedPartial);
            Console.WriteLine(partialString); // "a+%26+b" (only first 5 bytes encoded)

            // Encode with custom encoding options
            byte[] utf32Data = Encoding.UTF32.GetBytes("hello");
            byte[] utf32Encoded = Decorator.Enclose(utf32Data).UrlEncode(0, utf32Data.Length, o =>
            {
                o.Encoding = Encoding.UTF32;
            });

            // Handle empty byte arrays
            byte[] empty = Array.Empty<byte>();
            byte[] emptyEncoded = Decorator.Enclose(empty).UrlEncode();
            Console.WriteLine(emptyEncoded.Length); // 0

}}
}

```
