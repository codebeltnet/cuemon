---
uid: Cuemon.DelimitedStringOptions`1
example:
- *content
---

The following example demonstrates how to use <see cref="DelimitedStringOptions{T}"/> to configure the conversion of a sequence of objects into a delimited string.

```csharp
// Program.cs
using System;
using System.Globalization;
using Cuemon;

var options = new DelimitedStringOptions<int>
{
    Delimiter = " | ",
    StringConverter = number => number.ToString("X2", CultureInfo.InvariantCulture)
};

int[] numbers = { 1, 2, 3, 4 };
string hex = DelimitedString.Create(numbers, setup =>
{
    setup.Delimiter = options.Delimiter;
    setup.StringConverter = options.StringConverter;
});

Console.WriteLine(hex);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f993953cb86ea051a807bab5"><a href="~/run-actions/f993953cb86ea051a807bab5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f993953cb86ea051a807bab5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
