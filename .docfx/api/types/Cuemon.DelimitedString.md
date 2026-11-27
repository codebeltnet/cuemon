---
uid: Cuemon.DelimitedString
example:
- *content
---

The following example shows how to create a delimited string from an array of integers and split it back into parts using `DelimitedString`. The delimiter and string converter are configured through setup options.

```csharp
// Program.cs
using System;
using System.Globalization;
using Cuemon;

var numbers = new[] { 1, 2, 3, 4, 5 };
string csv = DelimitedString.Create(numbers, o =>
{
    o.Delimiter = ",";
    o.StringConverter = value => value.ToString();
});
Console.WriteLine(csv); // "1,2,3,4,5"

string[] parts = DelimitedString.Split(csv, o =>
{
    o.Delimiter = ",";
});
Console.WriteLine(string.Join(" | ", parts)); // "1 | 2 | 3 | 4 | 5"
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="eb06a734e14fd73cde88b074"><a href="~/run-actions/eb06a734e14fd73cde88b074/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/eb06a734e14fd73cde88b074/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
