---
uid: Cuemon.AspNetCore.Http.Headers.ExpiresHeaderValue
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Http.Headers.ExpiresHeaderValue"/> to specify when a response should be considered stale.

```csharp
using System;
using Cuemon.AspNetCore.Http.Headers;

namespace MyApp.Examples;

public class ExpiresHeaderValueExample
{
    public void Demonstrate()
    {
        // Create an Expires header value that makes the response stale after 1 hour
        var expires = new ExpiresHeaderValue(TimeSpan.FromHours(1));

        // The ToString() method produces the RFC 1123 format
        string headerValue = expires.ToString();
        Console.WriteLine($"Expires: {headerValue}");

        // Output example: "Expires: Thu, 17 Jun 2026 12:00:00 GMT"

}
}

```
