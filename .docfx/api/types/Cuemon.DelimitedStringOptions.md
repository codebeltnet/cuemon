---
uid: Cuemon.DelimitedStringOptions
example:
- *content
---

The following example demonstrates how to use <see cref="DelimitedStringOptions"/> to configure custom delimiters and qualifiers when parsing delimited strings using <see cref="DelimitedString"/>.

```csharp
// Program.cs
using System;
using Cuemon; // for DelimitedStringOptions, DelimitedString

// Default options: comma delimiter, double-quote qualifier
var defaultOptions = new DelimitedStringOptions();
Console.WriteLine($"Delimiter: '{defaultOptions.Delimiter}'"); // ','
Console.WriteLine($"Qualifier: '{defaultOptions.Qualifier}'"); // '"'

// Custom tab-delimited options
var tabOptions = new DelimitedStringOptions
{
    Delimiter = "\t",
    Qualifier = "'"
};

// Parse a tab-delimited line using the setup action
string tabLine = "Alice\t30\tNew York";
string[] fields = DelimitedString.Split(tabLine, o =>
{
    o.Delimiter = tabOptions.Delimiter;
    o.Qualifier = tabOptions.Qualifier;
});
Console.WriteLine($"Fields: {string.Join(", ", fields)}"); // Alice, 30, New York

// Parse a pipe-delimited line
string pipeLine = "'Bob'|'25'|'London'";
fields = DelimitedString.Split(pipeLine, o =>
{
    o.Delimiter = "|";
    o.Qualifier = "'";
});
Console.WriteLine($"Fields: {string.Join(", ", fields)}"); // 'Bob', '25', 'London'
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="37871fdd924c8ac21af4050d"><a href="~/run-actions/37871fdd924c8ac21af4050d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/37871fdd924c8ac21af4050d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
