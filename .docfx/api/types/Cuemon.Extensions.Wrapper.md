---
uid: Cuemon.Extensions.Wrapper
example:
- *content
---

The following example demonstrates how to wrap values using `Wrapper<T>` to access the inner instance, its type, and a parsed string representation. It shows wrapping both an integer and a string value.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Extensions;

var wrapped = new Wrapper<int>(42);
Console.WriteLine(wrapped.Instance);
Console.WriteLine(wrapped.InstanceType);

string parsed = Wrapper.ParseInstance(wrapped);
Console.WriteLine(parsed);

int asInt = wrapped.InstanceAs<int>();
Console.WriteLine(asInt);

var wrappedString = new Wrapper<string>("Hello, World!");
Console.WriteLine(Wrapper.ParseInstance(wrappedString));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="cef1735e26b87a3ce1022981"><a href="~/run-actions/cef1735e26b87a3ce1022981/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/cef1735e26b87a3ce1022981/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
