---
uid: Cuemon.Extensions.AspNetCore.Diagnostics.ServiceProviderExtensions
example:
- *content
---

The following example demonstrates how to retrieve all registered `IHttpExceptionDescriptorResponseFormatter` services from the service provider. The sample registers a formatter in a ServiceCollection, resolves the exception response formatters from the provider, and prints the count and formatter option type.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Net.Http;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var formatter = new HttpExceptionDescriptorResponseFormatter<ExampleFormatterOptions>(new ExampleFormatterOptions());
services.AddSingleton(formatter.GetType(), formatter);

using var provider = services.BuildServiceProvider();
var formatters = provider.GetExceptionResponseFormatters().ToList();

Console.WriteLine(formatters.Count);
Console.WriteLine(formatters[0].GetType().GenericTypeArguments[0].Name);

public sealed class ExampleFormatterOptions : IContentNegotiation, IExceptionDescriptorOptions
{
    public IReadOnlyCollection<MediaTypeHeaderValue> SupportedMediaTypes { get; } =
        new[] { new MediaTypeHeaderValue("application/json") };

    public FaultSensitivityDetails SensitivityDetails { get; set; }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="589bb17f8b48f3520f6695b5"><a href="~/run-actions/589bb17f8b48f3520f6695b5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/589bb17f8b48f3520f6695b5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
