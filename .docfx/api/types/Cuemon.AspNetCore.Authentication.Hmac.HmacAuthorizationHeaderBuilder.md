---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthorizationHeaderBuilder
example:
- *content
---

The following example demonstrates how to build an HMAC authorization header for an outgoing HTTP request.

```csharp
using System;
using System.Globalization;
using System.Net.Http;
using Cuemon.AspNetCore.Authentication.Hmac;

namespace MyApp.Examples;

public static class HmacAuthorizationHeaderBuilderExample
{
    public static void Demonstrate()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.cuemon.net/resource?name=Agent");
        request.Headers.Date = DateTimeOffset.Parse("2022-07-10T12:50:42Z", CultureInfo.InvariantCulture);
        request.Headers.Host = "api.cuemon.net";

        var builder = new HmacAuthorizationHeaderBuilder()
            .AddFromRequest(request)
            .AddClientId("Agent-Api")
            .AddClientSecret("Test")
            .AddCredentialScope("20220710/us-east-1/docs/request");

        var header = builder.Build();

        Console.WriteLine(header.ToString());
    }
}

```
