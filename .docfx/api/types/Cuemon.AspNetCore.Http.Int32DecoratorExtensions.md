---
uid: Cuemon.AspNetCore.Http.Int32DecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.AspNetCore.Http.Int32DecoratorExtensions> extension methods to classify HTTP status codes through the <xref:Cuemon.Decorator`1> pattern.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.AspNetCore.Http;

int statusCode = 500;

// Wrap the int in a Decorator and use the extension methods
var decorator = Decorator.Enclose(statusCode);

bool isInfo = decorator.IsInformationStatusCode();
bool isSuccess = decorator.IsSuccessStatusCode();
bool isRedirect = decorator.IsRedirectionStatusCode();
bool isClientError = decorator.IsClientErrorStatusCode();
bool isServerError = decorator.IsServerErrorStatusCode();
bool isNotModified = decorator.IsNotModifiedStatusCode();

Console.WriteLine($"HTTP {statusCode}:");
Console.WriteLine($"  Informational (100-199): {isInfo}");
Console.WriteLine($"  Successful (200-299): {isSuccess}");
Console.WriteLine($"  Redirection (300-399): {isRedirect}");
Console.WriteLine($"  Client Error (400-499): {isClientError}");
Console.WriteLine($"  Server Error (500-599): {isServerError}");
Console.WriteLine($"  Not Modified (304): {isNotModified}");

// Use with a success status code
int ok = 200;
var okDecorator = Decorator.Enclose(ok);
Console.WriteLine($"\n{ok} is success: {okDecorator.IsSuccessStatusCode()}");   // True
Console.WriteLine($"404 is client error: {Decorator.Enclose(404).IsClientErrorStatusCode()}"); // True
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e6c441a1a44682b3077c0dfe"><a href="~/run-actions/e6c441a1a44682b3077c0dfe/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e6c441a1a44682b3077c0dfe/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
