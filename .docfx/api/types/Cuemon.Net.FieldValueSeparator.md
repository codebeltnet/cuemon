---
uid: Cuemon.Net.FieldValueSeparator
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Net.FieldValueSeparator"/> to specify the separator for query string key-value pairs.

```csharp
// Program.cs
using System;
using Cuemon.Net;

var separator = FieldValueSeparator.Ampersand;

switch (separator)
{
    case FieldValueSeparator.Ampersand:
        Console.WriteLine("Using & separator for query string parameters (default).");
        break;
    case FieldValueSeparator.Semicolon:
        Console.WriteLine("Using ; separator for query string parameters.");
        break;

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f04cce3d8591dbab7fecf590"><a href="~/run-actions/f04cce3d8591dbab7fecf590/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f04cce3d8591dbab7fecf590/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
