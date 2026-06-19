---
uid: Cuemon.Extensions.Net.ByteArrayExtensions
example:
- *content
---

The following example demonstrates how to URL-encode byte array data for safe HTTP transmission using ByteArrayExtensions, with support for partial encoding and custom character encodings.

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
