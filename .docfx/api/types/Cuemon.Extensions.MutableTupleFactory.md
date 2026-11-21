---
uid: Cuemon.Extensions.MutableTupleFactory
example:
- *content
---

The following example demonstrates how to create mutable tuples with zero to five arguments using `MutableTupleFactory`. It shows accessing tuple values and constructing tuples of various sizes.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Extensions;

var zero = MutableTupleFactory.CreateZero();

var one = MutableTupleFactory.CreateOne(42);
Console.WriteLine(one.Arg1);

var two = MutableTupleFactory.CreateTwo("Alice", 30);
Console.WriteLine($"{two.Arg1} is {two.Arg2} years old");

var three = MutableTupleFactory.CreateThree(1, 2, 3);

var five = MutableTupleFactory.CreateFive('a', 'b', 'c', 'd', 'e');
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6ea81c9383c4ffb995fb0bfb"><a href="~/run-actions/6ea81c9383c4ffb995fb0bfb/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6ea81c9383c4ffb995fb0bfb/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
