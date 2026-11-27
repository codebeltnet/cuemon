---
uid: Cuemon.AspNetCore.Mvc.Filters.ModelBinding.DisableModelBindingAttribute
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Mvc.Filters.ModelBinding.DisableModelBindingAttribute"/> to remove a specific `IValueProviderFactory` during `OnResourceExecutionAsync`, such as disabling form value binding when handling file uploads.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Filters.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

var attribute = new DisableModelBindingAttribute(typeof(FormValueProviderFactory));
var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
var factories = new List<IValueProviderFactory>
{
    new FormValueProviderFactory(),
    new QueryStringValueProviderFactory()
};
var context = new ResourceExecutingContext(actionContext, new List<IFilterMetadata>(), factories);

Console.WriteLine($"Factories before: {context.ValueProviderFactories.Count}");
await attribute.OnResourceExecutionAsync(context, () =>
    Task.FromResult(new ResourceExecutedContext(actionContext, new List<IFilterMetadata>())));
Console.WriteLine($"Factories after: {context.ValueProviderFactories.Count}");
Console.WriteLine($"Remaining factory: {context.ValueProviderFactories[0].GetType().Name}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a0fb03144e0da7b6d10f0cc8"><a href="~/run-actions/a0fb03144e0da7b6d10f0cc8/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a0fb03144e0da7b6d10f0cc8/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
