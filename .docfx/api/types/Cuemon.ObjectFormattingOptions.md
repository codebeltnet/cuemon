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

// Direct instantiation of ObjectFormattingOptions
var options = new ObjectFormattingOptions
{
    FormatProvider = new CultureInfo("da-DK")
};

var value = "1234.56";

// Convert the string to a double using Danish formatting
var result = Decorator.Enclose((object)value)
    .ChangeType<double>(o =>
    {
        o.FormatProvider = new CultureInfo("da-DK");
    });

Console.WriteLine(result); // Output: 1234.56
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="71b4339e023d93125f75d02a"><a href="~/run-actions/71b4339e023d93125f75d02a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/71b4339e023d93125f75d02a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
