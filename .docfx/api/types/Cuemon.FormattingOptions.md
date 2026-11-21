---
uid: Cuemon.FormattingOptions
example:
- *content
---

The following example demonstrates how to configure a <see cref="Cuemon.FormattingOptions"/> with a custom format provider to control numeric formatting.

```csharp
// Program.cs
using System;
using System.Globalization;
using Cuemon;

var options = new FormattingOptions
{
    FormatProvider = new CultureInfo("da-DK")
};

var value = 1234.56;
var formatted = value.ToString("N2", options.FormatProvider);

Console.WriteLine(formatted); // Output depends on the culture (e.g., "1.234,56" for da-DK)
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="591cbb6bb8193856480e8739"><a href="~/run-actions/591cbb6bb8193856480e8739/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/591cbb6bb8193856480e8739/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
