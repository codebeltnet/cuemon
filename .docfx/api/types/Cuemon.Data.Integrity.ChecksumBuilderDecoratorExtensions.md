---
uid: Cuemon.Data.Integrity.ChecksumBuilderDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use `ChecksumBuilderDecoratorExtensions` to combine typed values with a `ChecksumBuilder` through the `IDecorator<T>` interface.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Data.Integrity;
using Cuemon.Security;

// Create a ChecksumBuilder wrapped in a Decorator
var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
var decorator = Decorator.Enclose(builder);

// Combine various typed values using the decorator extensions
decorator.CombineWith(42);             // int
decorator.CombineWith(3.14);           // double
decorator.CombineWith("tag");          // string

Console.WriteLine($"Combined checksum: {builder}");

// The extensions return the inner builder for chaining
var combined = decorator.CombineWith(12345L); // long
Console.WriteLine($"Returned type: {combined.GetType().Name}");

// Extension methods also work with short, float, ushort, uint, ulong
var shortDecorator = Decorator.Enclose(new ChecksumBuilder(() => HashFactory.CreateFnv32()));
shortDecorator.CombineWith((short)100);
shortDecorator.CombineWith(3.14f);
shortDecorator.CombineWith(42u);

Console.WriteLine($"Short builder: {shortDecorator.Inner}");

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="976a19c257adeb1ca001acf4"><a href="~/run-actions/976a19c257adeb1ca001acf4/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/976a19c257adeb1ca001acf4/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
