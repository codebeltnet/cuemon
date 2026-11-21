---
uid: Cuemon.ActionFactory`1
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.ActionFactory{TTuple}"/> to wrap and invoke a delegate with n-tuple arguments.

```csharp
// Program.cs
using System;
using Cuemon;

// Create a mutable tuple with string and int arguments
var tuple = new MutableTuple<string, int>("Hello", 42);

// Define an action that processes the tuple
void Process(MutableTuple<string, int> t)
{
    Console.WriteLine($"Message: {t.Arg1}, Value: {t.Arg2}");
}

// Wrap the action and tuple in an ActionFactory
var factory = new ActionFactory<MutableTuple<string, int>>(Process, tuple);

// Inspect factory state
Console.WriteLine(factory.HasDelegate);   // True
Console.WriteLine(factory.GenericArguments.Arg1); // "Hello"
Console.WriteLine(factory.GenericArguments.Arg2); // 42

// Invoke the wrapped delegate
factory.ExecuteMethod(); // Output: "Message: Hello, Value: 42"

// Create a clone for safe concurrent use
var clone = factory.Clone() as ActionFactory<MutableTuple<string, int>>;
clone?.ExecuteMethod();

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ca6ef14e13670f55a7f76f10"><a href="~/run-actions/ca6ef14e13670f55a7f76f10/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ca6ef14e13670f55a7f76f10/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
