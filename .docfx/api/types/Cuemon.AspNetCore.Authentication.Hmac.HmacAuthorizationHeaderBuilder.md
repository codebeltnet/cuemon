---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthorizationHeaderBuilder
example:
- *content
---

The following example demonstrates how to build an HMAC authorization header for an outgoing HTTP request.

```csharp
// Program.cs
using System;
using System.Globalization;
using System.Net.Http;
using Cuemon.AspNetCore.Authentication.Hmac;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0323d469e14a42e0124cc6c6"><a href="~/run-actions/0323d469e14a42e0124cc6c6/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0323d469e14a42e0124cc6c6/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
