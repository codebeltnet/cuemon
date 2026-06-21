---
uid: Cuemon.AspNetCore.Diagnostics.HttpFaultResolverDecoratorExtensions
example:
- *content
---

The following example demonstrates how to register fault resolvers using the decorator pattern over a list of `HttpFaultResolver`.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;

using Cuemon;
namespace Examples;

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

```
