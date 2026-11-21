---
uid: Cuemon.StringFactory
example:
- *content
---

The following example demonstrates how to use `StringFactory` to generate hexadecimal, binary, URL-safe Base64, protocol-relative URL, and URI scheme strings from .NET data types.

```csharp
// Program.cs
using System;
using Cuemon;

// Convert bytes to hexadecimal string
byte[] binaryData = { 0x0F, 0xA0, 0x01 };
string hex = StringFactory.CreateHexadecimal(binaryData);
Console.WriteLine(hex); // 0fa001

// Convert string to hexadecimal representation
string hexFromString = StringFactory.CreateHexadecimal("Hello");
Console.WriteLine(hexFromString); // 48656c6c6f

// Create binary digit string from bytes
string binary = StringFactory.CreateBinaryDigits(new byte[] { 0, 1, 255 });
Console.WriteLine(binary); // 000000000000000111111111

// Create URL-safe Base64 string
string urlSafe = StringFactory.CreateUrlEncodedBase64(new byte[] { 251, 255 });
Console.WriteLine(urlSafe); // -_8

// Create a protocol-relative URL (// prefix replaces https://)
string relativeUrl = StringFactory.CreateProtocolRelativeUrl(
    new Uri("https://www.cuemon.net/about"));
Console.WriteLine(relativeUrl); // //www.cuemon.net/about

// Get string representation of a URI scheme enum
string scheme = StringFactory.CreateUriScheme(UriScheme.Https);
Console.WriteLine(scheme); // https
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="fdb368f878c6231684d661ad"><a href="~/run-actions/fdb368f878c6231684d661ad/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/fdb368f878c6231684d661ad/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
