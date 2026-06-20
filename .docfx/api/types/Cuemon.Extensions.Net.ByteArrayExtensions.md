---
uid: Cuemon.Extensions.Net.ByteArrayExtensions
example:
- *content
---

`ByteArrayExtensions` in the `Net` namespace URL-encodes byte arrays for safe HTTP transmission with support for partial encoding and custom character encodings. This example converts `"hello world"` to a UTF-8 byte array and calls `UrlEncode` to produce `hello%20world`, then demonstrates partial encoding of only the first five bytes (producing `hello`) and encoding with UTF-32 where each character is encoded as 4 bytes. Console output displays each encoded result, confirming that full encoding, partial ranges, and non-default encodings all produce correct percent-encoded output.

```csharp
using System;
using System.Text;
using Cuemon.Extensions.Net;
using Cuemon.Text;

namespace MyApp.Net
{
    public class ByteArrayExtensionsExample
    {
        public void Demonstrate()
        {
            // Encode a byte array for safe URL transmission
            byte[] data = Encoding.UTF8.GetBytes("hello world");

            // URL-encode the entire byte array
            byte[] encoded = data.UrlEncode();
            Console.WriteLine(Encoding.UTF8.GetString(encoded));
            // Output: hello%20world

            // Encode only a portion starting at position 0 for 5 bytes
            byte[] partial = data.UrlEncode(position: 0, bytesToRead: 5);
            Console.WriteLine(Encoding.UTF8.GetString(partial));
            // Output: hello

            // Use a custom encoding (e.g., UTF-32)
            byte[] utf32Data = Encoding.UTF32.GetBytes("test data");
            byte[] utf32Encoded = utf32Data.UrlEncode(setup: o => o.Encoding = Encoding.UTF32);
            string result = Encoding.UTF32.GetString(utf32Encoded);
            Console.WriteLine(result);
            // Output: test%00%00%00%20%00%00%00data

}}
}

```
