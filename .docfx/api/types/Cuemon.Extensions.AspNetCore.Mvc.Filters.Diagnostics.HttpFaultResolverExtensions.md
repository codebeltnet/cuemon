---
uid: Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics.HttpFaultResolverExtensions
example:
- *content
---

The following example builds a resolver list the same way MVC fault handling does internally: map known exception types to HTTP-friendly descriptors, then ask a resolver to translate the thrown exception.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics.DocExamples;

public sealed class HttpFaultResolverExtensionsExample
{
    public void Describe()
    {
        var resolvers = new List<HttpFaultResolver>()
            .AddHttpFaultResolver<BadRequestException>(
                message: "The request payload could not be processed.",
                helpLink: new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400"))
            .AddHttpFaultResolver<TooManyRequestsException>(
                exception => new HttpExceptionDescriptor(exception),
                exception => exception is TooManyRequestsException);

        var resolved = resolvers[1].TryResolveFault(new TooManyRequestsException(), out var descriptor);

        Console.WriteLine($"{resolved}: {descriptor.StatusCode} {descriptor.Code}");
    }
}
```
