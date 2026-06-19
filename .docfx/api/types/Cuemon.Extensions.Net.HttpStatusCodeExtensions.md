---
uid: Cuemon.Extensions.Net.HttpStatusCodeExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.Net.HttpStatusCodeExtensions> extension methods to classify HTTP status codes by range directly from <see cref="System.Net.HttpStatusCode"/> values.

```csharp
using System;
using System.Net;
using Cuemon.Extensions.Net;

namespace DocExamples
{
    public static class HttpStatusCodeExtensionsExample
    {
        public static void Main()
        {
            HttpStatusCode statusCode = HttpStatusCode.NotFound;

            // Check the status code HTTP range
            bool isInfo = statusCode.IsInformationStatusCode();
            bool isSuccess = statusCode.IsSuccessStatusCode();
            bool isRedirect = statusCode.IsRedirectionStatusCode();
            bool isClientError = statusCode.IsClientErrorStatusCode();
            bool isServerError = statusCode.IsServerErrorStatusCode();

            Console.WriteLine($"HTTP {(int)statusCode} ({statusCode}):");
            Console.WriteLine($"  Informational (100-199): {isInfo}");
            Console.WriteLine($"  Successful (200-299): {isSuccess}");
            Console.WriteLine($"  Redirection (300-399): {isRedirect}");
            Console.WriteLine($"  Client Error (400-499): {isClientError}");
            Console.WriteLine($"  Server Error (500-599): {isServerError}");

            // Verify common status codes
            Console.WriteLine($"\n200 OK is success: {HttpStatusCode.OK.IsSuccessStatusCode()}");
            Console.WriteLine($"301 Moved is redirect: {HttpStatusCode.MovedPermanently.IsRedirectionStatusCode()}");
            Console.WriteLine($"403 Forbidden is client error: {HttpStatusCode.Forbidden.IsClientErrorStatusCode()}");
            Console.WriteLine($"500 Error is server error: {HttpStatusCode.InternalServerError.IsServerErrorStatusCode()}");
            Console.WriteLine($"100 Continue is informational: {HttpStatusCode.Continue.IsInformationStatusCode()}");

}}
}

```
