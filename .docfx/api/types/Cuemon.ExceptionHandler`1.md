---
uid: Cuemon.ExceptionHandler`1
example:
- *content
---

The following example demonstrates how to use <see cref="ExceptionHandler{TException}"/> as the intermediate step in the fluent exception-triggering chain.

```csharp
using System;
using Cuemon; // for ExceptionCondition, ExceptionHandler, ExceptionInvoker

namespace MyApp.Examples;

public class ExceptionHandlerExample
{
    public void Demonstrate()
    {
        // Build the chain: Condition -> Handler -> Invoker
        ExceptionInvoker<ArgumentException> invoker = new ExceptionCondition<ArgumentException>()
            .IsTrue(() => string.IsNullOrEmpty(Environment.GetEnvironmentVariable("API_KEY")))
            .Create(() => new ArgumentException("API_KEY environment variable is not set."));

        // When TryThrow is called, it evaluates the condition
        // and throws the exception created by the handler if the condition matches
        invoker.TryThrow();

        // The handler can be stored and reused to create different invokers
        ExceptionHandler<InvalidOperationException> handler =
            new ExceptionCondition<InvalidOperationException>().IsTrue(() => true);

        // Create different invokers from the same handler
        var invokerA = handler.Create(() => new InvalidOperationException("Reason A"));
        var invokerB = handler.Create(() => new InvalidOperationException("Reason B"));

        // invokerA.TryThrow(); // throws "Reason A"
        // invokerB.TryThrow(); // throws "Reason B"

}
}

```
