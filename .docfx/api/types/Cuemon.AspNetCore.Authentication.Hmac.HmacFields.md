---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacFields
example:
- *content
---

The following example demonstrates how to reference the field constants of `HmacFields` when building an HMAC authorization header.

```csharp
using System;
using System.Net.Http;
using Cuemon.AspNetCore.Authentication.Hmac;

namespace MyApp.Examples;

public static class HmacFieldsExample
{
    public static void Demonstrate()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.example.com/resource");
        request.Headers.Date = DateTimeOffset.UtcNow;
        request.Headers.Host = "api.example.com";

        var builder = new HmacAuthorizationHeaderBuilder()
            .AddFromRequest(request)
            .AddClientId("my-client")
            .AddClientSecret("my-secret")
            .AddCredentialScope("20250101/us-east-1/service/aws4_request");

        var header = builder.Build();

        Console.WriteLine(HmacFields.Scheme);
        Console.WriteLine(HmacFields.SignedHeaders);
        Console.WriteLine(HmacFields.CanonicalRequest);
    }
}

```
