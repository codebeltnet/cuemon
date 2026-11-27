---
uid: Cuemon.Extensions.TesterFuncFactory
example:
- *content
---

The following example demonstrates how to create a `TesterFuncFactory` from a TryParse-style delegate. It shows both a successful parse and a failure case, printing the parsed value or fallback accordingly.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Extensions;

TesterFunc<string, int, bool> tryParse = (string input, out int result) => int.TryParse(input, out result);

var factory = TesterFuncFactory.Create(tryParse, "42");
bool success = factory.ExecuteMethod(out int value);
Console.WriteLine($"Parsed: {value}, Success: {success}");

var failFactory = TesterFuncFactory.Create(tryParse, "not-a-number");
bool failed = failFactory.ExecuteMethod(out int fallback);
Console.WriteLine($"Fallback: {fallback}, Success: {failed}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="06baa6d44a12c6ec8db7e716"><a href="~/run-actions/06baa6d44a12c6ec8db7e716/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/06baa6d44a12c6ec8db7e716/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
