---
uid: Cuemon.AspNetCore.Mvc.Filters.ModelBinding.DisableModelBindingAttribute
example:
- *content
---

The following example demonstrates how to use the <xref cref="Cuemon.AspNetCore.Mvc.Filters.ModelBinding.DisableModelBindingAttribute"/> to disable a specific model binding value provider, such as when handling file uploads to prevent form value binding.

```csharp
using System;
using Cuemon.AspNetCore.Mvc.Filters.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyApp.Examples;

[DisableModelBinding(typeof(FormValueProviderFactory))]
public class FileUploadController : Controller
{
    [HttpPost("/upload")]
    public IActionResult Upload()
    {
        return Ok("File upload processed.");
    }
}

public class DisableModelBindingAttributeDirectUsage
{
    public void Demonstrate()
    {
        // Direct instantiation of DisableModelBindingAttribute
        var attribute = new DisableModelBindingAttribute(typeof(FormValueProviderFactory));
        Console.WriteLine($"Disabled type: {attribute.ValueProviderFactoryType.Name}");
    }
}

```
