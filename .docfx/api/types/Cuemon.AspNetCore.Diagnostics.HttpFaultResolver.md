---
uid: Cuemon.AspNetCore.Diagnostics.HttpFaultResolver
example:
- *content
---

The following example demonstrates how to create an HTTP fault resolver that maps exceptions to HTTP error responses.

```csharp
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;

namespace MyApp.Examples;

public class HttpFaultResolverExample
{
    public void Demonstrate()
    {
        var resolver = new HttpFaultResolver(
            exception => exception is ArgumentNullException,
            exception => new HttpExceptionDescriptor(exception, 400, null, "Argument was null."));

        if (resolver.TryResolveFault(new ArgumentNullException("value"), out var descriptor))
        {
            Console.WriteLine(descriptor.StatusCode); // 400
        }

        var resolved = resolver.TryResolveFault(new InvalidOperationException(), out _);
        Console.WriteLine(resolved); // False
    }
}
```
