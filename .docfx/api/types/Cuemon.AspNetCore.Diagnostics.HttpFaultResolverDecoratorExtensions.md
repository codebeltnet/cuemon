---
uid: Cuemon.AspNetCore.Diagnostics.HttpFaultResolverDecoratorExtensions
example:
- *content
---

The following example demonstrates how to register fault resolvers using the decorator pattern over a list of `HttpFaultResolver` and evaluate exceptions against the registered resolvers.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Examples;

var resolvers = new FaultResolverRegistrationExample().RegisterResolvers();
Console.WriteLine($"Registered resolvers: {resolvers.Count}");

Exception[] exceptions =
[
    new NotFoundException("Item 42 does not exist."),
    new UnauthorizedException("Token expired.")
];

foreach (var ex in exceptions)
{
    foreach (var resolver in resolvers)
    {
        if (resolver.TryResolveFault(ex, out var descriptor))
        {
            Console.WriteLine($"{ex.GetType().Name} -> {descriptor.StatusCode} {descriptor.Code}: {descriptor.Message}");
        }
    }
}

namespace Examples
{
    public class FaultResolverRegistrationExample
    {
        public IList<HttpFaultResolver> RegisterResolvers()
        {
            var resolvers = new List<HttpFaultResolver>();

            // Add a resolver for NotFoundException (404)
            Decorator.Enclose(resolvers).AddHttpFaultResolver<NotFoundException>(
                message: "The requested resource was not found.",
                helpLink: new Uri("https://example.com/errors/404"));

            // Add a resolver for UnauthorizedException (401) with a custom code
            Decorator.Enclose(resolvers).AddHttpFaultResolver<UnauthorizedException>(
                statusCode: 401,
                code: "UNAUTHORIZED",
                message: "Authentication is required.");

            return resolvers;
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c538da1818bafa5627106e44"><a href="~/run-actions/c538da1818bafa5627106e44/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c538da1818bafa5627106e44/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
