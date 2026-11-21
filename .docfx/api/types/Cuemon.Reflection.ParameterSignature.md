---
uid: Cuemon.Reflection.ParameterSignature
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Reflection.ParameterSignature"/> to extract parameter information from a method.

```csharp
// Program.cs
using System;
using System.Linq;
using System.Reflection;
using Cuemon.Reflection;

        MethodInfo method = typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) });

        if (method != null)
        {
            var signatures = ParameterSignature.Parse(method).ToList();

            foreach (var signature in signatures)
            {
                Console.WriteLine($"Parameter: {signature.ParameterName}, Type: {signature.ParameterType.Name}");

}}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d206ece36dfda1c486b946ed"><a href="~/run-actions/d206ece36dfda1c486b946ed/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d206ece36dfda1c486b946ed/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
