---
uid: Cuemon.Alphanumeric
example:
- *content
---

The following example demonstrates how to use the `Alphanumeric` class to access predefined character sets. It prints the available character ranges for digits, uppercase letters, hexadecimal characters, and punctuation marks.

```csharp
// Program.cs
using System;
using Cuemon;

Console.WriteLine(Alphanumeric.Numbers);           // "0123456789"
Console.WriteLine(Alphanumeric.UppercaseLetters);  // "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
Console.WriteLine(Alphanumeric.Hexadecimal);       // "0123456789ABCDEF"
Console.WriteLine(Alphanumeric.PunctuationMarks);  // "!@#$%^&*()_-+=[{]};:<>|.,/?`~\"'"
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ebc3994820ebf6dd1adb3a71"><a href="~/run-actions/ebc3994820ebf6dd1adb3a71/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ebc3994820ebf6dd1adb3a71/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
