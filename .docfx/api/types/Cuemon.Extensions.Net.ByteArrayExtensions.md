---
uid: Cuemon.Extensions.Net.ByteArrayExtensions
example:
- *content
---

`ByteArrayExtensions` in the `Net` namespace URL-encodes byte arrays for safe HTTP transmission with support for partial encoding and custom character encodings. This example converts `"hello world"` to a UTF-8 byte array and calls `UrlEncode` to produce `hello+world`, then demonstrates partial encoding of only the first five bytes (producing `hello`) and encoding with UTF-32 where each character is encoded as 4 bytes. Console output displays each encoded result, confirming that full encoding, partial ranges, and non-default encodings all produce URL-encoded output.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Extensions.Net;
using Cuemon.Text;

// Encode a byte array for safe URL transmission
byte[] data = Encoding.UTF8.GetBytes("hello world");

// URL-encode the entire byte array
byte[] encoded = data.UrlEncode(0, data.Length);
Console.WriteLine(Encoding.UTF8.GetString(encoded));
// Output: hello+world

// Encode only a portion starting at position 0 for 5 bytes
byte[] partial = data.UrlEncode(position: 0, bytesToRead: 5);
Console.WriteLine(Encoding.UTF8.GetString(partial));
// Output: hello

// Use a custom encoding (e.g., UTF-32)
byte[] utf32Data = Encoding.UTF32.GetBytes("test data");
byte[] utf32Encoded = utf32Data.UrlEncode(0, utf32Data.Length, setup: o => o.Encoding = Encoding.UTF32);
string result = Encoding.UTF32.GetString(utf32Encoded);
Console.WriteLine(result);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e1310a4bc7533d43927f901c"><a href="~/run-actions/e1310a4bc7533d43927f901c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e1310a4bc7533d43927f901c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
