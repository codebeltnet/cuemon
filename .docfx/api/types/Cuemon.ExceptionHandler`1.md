---
uid: Cuemon.ExceptionHandler`1
example:
- *content
---

The following example demonstrates how to use <see cref="ExceptionHandler{TException}"/> as the intermediate step in the fluent exception-triggering chain.

```csharp
// Program.cs
using System;
using Cuemon;

// Build the chain: Condition -> Handler -> Invoker
ExceptionInvoker<ArgumentException> invoker = new ExceptionCondition<ArgumentException>()
    .IsTrue(() => string.IsNullOrEmpty(Environment.GetEnvironmentVariable("API_KEY")))
    .Create(() => new ArgumentException("API_KEY environment variable is not set."));

// When TryThrow is called, it evaluates the condition
// and throws the exception created by the handler if the condition matches
try
{
    invoker.TryThrow();
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message);
}

// The handler can be stored and reused to create different invokers
ExceptionHandler<InvalidOperationException> handler =
    new ExceptionCondition<InvalidOperationException>().IsTrue(() => true);

// Create different invokers from the same handler
var invokerA = handler.Create(() => new InvalidOperationException("Reason A"));
var invokerB = handler.Create(() => new InvalidOperationException("Reason B"));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c7a8b4566dfbb4c82fed3938"><a href="~/run-actions/c7a8b4566dfbb4c82fed3938/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c7a8b4566dfbb4c82fed3938/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
