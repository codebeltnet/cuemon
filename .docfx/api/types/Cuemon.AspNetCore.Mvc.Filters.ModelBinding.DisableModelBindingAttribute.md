---
uid: Cuemon.AspNetCore.Mvc.Filters.ModelBinding.DisableModelBindingAttribute
example:
- *content
---

The following example demonstrates how to use the <xref cref="Cuemon.AspNetCore.Mvc.Filters.ModelBinding.DisableModelBindingAttribute"/> to disable a specific model binding value provider, such as when handling file uploads to prevent form value binding.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc.Filters.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// Direct instantiation of DisableModelBindingAttribute
var attribute = new DisableModelBindingAttribute(typeof(FormValueProviderFactory));
Console.WriteLine($"Disabled type: {attribute.ValueProviderFactoryType.Name}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="afbbea59b34e56653be0dbec"><a href="~/run-actions/afbbea59b34e56653be0dbec/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/afbbea59b34e56653be0dbec/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
