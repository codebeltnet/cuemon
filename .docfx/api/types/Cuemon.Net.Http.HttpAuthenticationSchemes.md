---
uid: Cuemon.Net.Http.HttpAuthenticationSchemes
example:
- *content
---

The following example shows how to use the `HttpAuthenticationSchemes` constants to construct HTTP Authorization headers. It prints each scheme name and creates a Basic authentication header value.

```csharp
using System;
using System.Net.Http.Headers;
using System.Text;
using Cuemon.Net.Http;

namespace MyApp.Examples;

public static class HttpAuthenticationSchemesExample
{
    public static void Demonstrate()
    {
        // Use scheme constants to construct Authorization headers
        string basic = HttpAuthenticationSchemes.Basic;
        string bearer = HttpAuthenticationSchemes.Bearer;
        string digest = HttpAuthenticationSchemes.Digest;

        Console.WriteLine($"Basic: {basic}");
        Console.WriteLine($"Bearer: {bearer}");
        Console.WriteLine($"Digest: {digest}");

        // Example: create a Basic authentication header value
        string credentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes("user:password"));
        string authHeader = $"{basic} {credentials}";
        Console.WriteLine($"Authorization: {authHeader}");
    }
}
```
