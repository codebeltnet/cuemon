---
uid: Cuemon.Decorator`1
example:
- *content
---

The following example demonstrates how to use `Decorator<T>` to wrap a value and access it through the decorator pattern.

```csharp
// Program.cs
using System;
using Cuemon;

var numbers = new[] { 10, 20, 30 };
var decorator = Decorator.Enclose(numbers);

// Access the wrapped inner value through the Inner property
int[] inner = decorator.Inner;
Console.WriteLine(string.Join(", ", inner)); // "10, 20, 30"

// ArgumentName is set automatically when using EncloseToExpose
var withArgName = Decorator.EncloseToExpose(numbers);
Console.WriteLine(withArgName.ArgumentName); // "numbers"

// Syntactic sugar for type-level decoration
var syntactic = Decorator.Syntactic<string>();
Console.WriteLine(syntactic.Inner); // null (default)
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f6fb09153ea2c9d49a6b578e"><a href="~/run-actions/f6fb09153ea2c9d49a6b578e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f6fb09153ea2c9d49a6b578e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
