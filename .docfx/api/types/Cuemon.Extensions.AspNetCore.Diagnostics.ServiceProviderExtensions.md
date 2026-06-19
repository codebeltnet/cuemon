---
uid: Cuemon.Extensions.AspNetCore.Diagnostics.ServiceProviderExtensions
example:
- *content
---

The following example demonstrates how to retrieve all registered `IHttpExceptionDescriptorResponseFormatter` services from the service provider.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Examples;

public class ExceptionFormatterResolver
{
    public IEnumerable<IHttpExceptionDescriptorResponseFormatter> ResolveFormatters(IServiceProvider provider)
    {
        return provider.GetExceptionResponseFormatters();

}
}

```
