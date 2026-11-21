---
uid: Cuemon.AspNetCore.Diagnostics.HttpRequestEvidence
example:
- *content
---

`HttpRequestEvidence` captures HTTP request diagnostic data including headers, query parameters, form data, and the request body for logging and debugging scenarios. This example sets up a `DefaultHttpContext` with a POST request to `https://api.example.com/orders`, including an `Authorization` header, `X-Trace-Id`, a `status=pending` query string, form data with `customerId=42`, and a captured request body. Key steps include constructing an `HttpRequestEvidence` from the request and supplying a custom body converter delegate that redacts sensitive values by replacing `"42"` with `"***"`. Console output displays each evidence property (location, method, headers, query, form, body, and redacted body).

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cuemon.AspNetCore.Diagnostics;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;

var context = new DefaultHttpContext();
context.Request.Scheme = "https";
context.Request.Host = new HostString("api.example.com");
context.Request.Path = "/orders";
context.Request.QueryString = new QueryString("?status=pending");
context.Request.Method = HttpMethods.Post;
context.Request.ContentType = "application/x-www-form-urlencoded";
context.Request.Headers["Authorization"] = "Bearer eyJhbGci...";
context.Request.Headers["X-Trace-Id"] = "abc-123";
context.Request.Form = new FormCollection(
    new Dictionary<string, StringValues>
    {
        { "customerId", "42" }
    });

// Capture the request body so HttpRequestEvidence can retrieve it
var bodyBytes = Encoding.UTF8.GetBytes("customerId=42");
context.Items[HttpRequestEvidence.HttpContextItemsKeyForCapturedRequestBody] =
    new MemoryStream(bodyBytes);

var evidence = new HttpRequestEvidence(context.Request);

Console.WriteLine($"Location: {evidence.Location}");
Console.WriteLine($"Method: {evidence.Method}");
Console.WriteLine($"Auth Header: {evidence.Headers["Authorization"]}");
Console.WriteLine($"Query: status={evidence.Query["status"]}");
Console.WriteLine($"Form: customerId={evidence.Form["customerId"]}");
Console.WriteLine($"Body: {evidence.Body}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="705b005f353c871e56681e44"><a href="~/run-actions/705b005f353c871e56681e44/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/705b005f353c871e56681e44/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
