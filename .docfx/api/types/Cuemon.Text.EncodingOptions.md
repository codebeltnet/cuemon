---
uid: Cuemon.Text.EncodingOptions
example:
- *content
---

The following example demonstrates how to use <see cref="EncodingOptions"/> to control encoding behavior, including preamble handling and encoding selection.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Text; // for EncodingOptions, PreambleSequence

// Create options with UTF-8 encoding, removing the BOM preamble
var options = new EncodingOptions
{
    Encoding = Encoding.UTF8,
    Preamble = PreambleSequence.Remove
};
Console.WriteLine(options.Encoding.EncodingName); // Unicode (UTF-8)
Console.WriteLine(options.Preamble);              // Remove

// Create options that keep the preamble
var preserveOptions = new EncodingOptions
{
    Encoding = Encoding.Unicode,
    Preamble = PreambleSequence.Keep
};
Console.WriteLine(preserveOptions.Preamble); // Keep

// Encoding property validates for null
// options.Encoding = null; // throws ArgumentNullException
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="67477e6fd1fe98e8658f376f"><a href="~/run-actions/67477e6fd1fe98e8658f376f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/67477e6fd1fe98e8658f376f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
