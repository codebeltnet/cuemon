---
uid: Cuemon.Extensions.Net.HttpStatusCodeExtensions
example:
- *content
---

`HttpStatusCodeExtensions` provides extension methods for `HttpStatusCode` that classify status codes by HTTP range: informational (100–199), success (200–299), redirection (300–399), client error (400–499), and server error (500–599). This example evaluates `NotFound` (404), `OK` (200), `MovedPermanently` (301), `Forbidden` (403), `InternalServerError` (500), and `Continue` (100) using methods like `IsClientErrorStatusCode`, `IsSuccessStatusCode`, `IsRedirectionStatusCode`, and `IsServerErrorStatusCode`. Console output confirms each classification, such as `200 OK is success: True` and `404 NotFound is client error: True`.

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
