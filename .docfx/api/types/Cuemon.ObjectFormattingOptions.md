---
uid: Cuemon.ObjectFormattingOptions
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.ObjectFormattingOptions"/> to customize the conversion of an object to a different type via <see cref="ObjectDecoratorExtensions.ChangeType{T}"/>.

```csharp
// Program.cs
using System;
using System.Globalization;
using Cuemon;

var value = "1234,56";

var options = new ObjectFormattingOptions
{
    FormatProvider = new CultureInfo("da-DK")
};

// Convert the string to a double using Danish formatting (comma as decimal separator)
var result = Decorator.Enclose((object)value)
    .ChangeType<double>(o =>
    {
        o.FormatProvider = options.FormatProvider;
    });

Console.WriteLine(result.ToString(CultureInfo.InvariantCulture)); // Output: 1234.56
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f0022cc9c50658cad107c485"><a href="~/run-actions/f0022cc9c50658cad107c485/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f0022cc9c50658cad107c485/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
