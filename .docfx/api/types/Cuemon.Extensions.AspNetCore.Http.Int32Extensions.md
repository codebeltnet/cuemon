---
uid: Cuemon.Extensions.AspNetCore.Http.Int32Extensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.AspNetCore.Http.Int32Extensions> extension methods to classify HTTP status codes by range.

```csharp
using System;
using Cuemon.Extensions.AspNetCore.Http;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {

        int statusCode = 404;

        // Check the status code range
        bool isInfo = statusCode.IsInformationStatusCode();
        bool isSuccess = statusCode.IsSuccessStatusCode();
        bool isRedirect = statusCode.IsRedirectionStatusCode();
        bool isClientError = statusCode.IsClientErrorStatusCode();
        bool isServerError = statusCode.IsServerErrorStatusCode();
        bool isNotModified = statusCode.IsNotModifiedStatusCode();

        Console.WriteLine($"HTTP {statusCode}:");
        Console.WriteLine($"  Informational: {isInfo}");
        Console.WriteLine($"  Success: {isSuccess}");
        Console.WriteLine($"  Redirection: {isRedirect}");
        Console.WriteLine($"  Client Error: {isClientError}");
        Console.WriteLine($"  Server Error: {isServerError}");
        Console.WriteLine($"  Not Modified: {isNotModified}");

        // Use with common HTTP status codes
        int ok = 200;
        int notFound = 404;
        int serverError = 500;

        Console.WriteLine($"\n{ok} is success: {ok.IsSuccessStatusCode()}");
        Console.WriteLine($"{notFound} is client error: {notFound.IsClientErrorStatusCode()}");
        Console.WriteLine($"{serverError} is server error: {serverError.IsServerErrorStatusCode()}");

}
}

```
