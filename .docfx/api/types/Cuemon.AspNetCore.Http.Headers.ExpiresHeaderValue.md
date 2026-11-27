---
uid: Cuemon.AspNetCore.Http.Headers.ExpiresHeaderValue
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Http.Headers.ExpiresHeaderValue"/> to specify when a response should be considered stale.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http.Headers;

// Create an Expires header value that makes the response stale after 1 hour
var expires = new ExpiresHeaderValue(TimeSpan.FromHours(1));

// The ToString() method produces the RFC 1123 format
string headerValue = expires.ToString();
Console.WriteLine($"Expires: {headerValue}");

// Output example: "Expires: Thu, 17 Jun 2026 12:00:00 GMT"
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="76e1464c0ea7a7e769d7b323"><a href="~/run-actions/76e1464c0ea7a7e769d7b323/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/76e1464c0ea7a7e769d7b323/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
