---
uid: Cuemon.AspNetCore.Diagnostics.HttpRequestEvidence
example:
- *content
---

The following example demonstrates how to capture HTTP request evidence, including headers, query parameters, form data, and the request body, for diagnostic purposes.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cuemon.AspNetCore.Diagnostics;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;

namespace MyApp.Diagnostics
{
    public class HttpRequestEvidenceExample
    {
        public void Demonstrate()
        {
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

            // Provide a custom body converter to redact sensitive data
            var redacted = new HttpRequestEvidence(context.Request,
                stream => new StreamReader(stream).ReadToEnd().Replace("42", "***"));
            Console.WriteLine($"Redacted Body: {redacted.Body}");

}}
}

```
