---
uid: Cuemon.Decorator
example:
- *content
---

The following example demonstrates how to wrap values using `Decorator.Enclose` and related methods. It shows accessing the inner value, argument name, syntactic default, and raw (nullable) wrapping.

```csharp
// Program.cs
using System;
using Cuemon;

var wrappedString = Decorator.Enclose("Hello, World!");
Console.WriteLine(wrappedString.Inner); // "Hello, World!"
Console.WriteLine(wrappedString.ArgumentName); // ""

var withArg = Decorator.EncloseToExpose("test", argumentName: "myArg");
Console.WriteLine(withArg.ArgumentName); // "myArg"

var syntactic = Decorator.Syntactic<DateTime>();
Console.WriteLine(syntactic.Inner); // "01-01-0001 00:00:00" (default(DateTime))

var raw = Decorator.RawEnclose<string>(null);
Console.WriteLine(raw.Inner is null); // True
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e8328d9b8eef1bc11a24482b"><a href="~/run-actions/e8328d9b8eef1bc11a24482b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e8328d9b8eef1bc11a24482b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
