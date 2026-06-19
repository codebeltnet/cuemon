---
uid: Cuemon.ExceptionInvoker`2
example:
- *content
---

The following example demonstrates how to use the generic <xref cref="Cuemon.ExceptionInvoker{TException, TResult}"/> class to conditionally throw an exception using a tester function with an out value.

```csharp
using System;
using Cuemon;

namespace MyApp.Examples;

public class ExceptionInvokerOfT2Example
{
    public void Demonstrate()
    {
        // Create a tester that extracts a value from a configuration string
        // TesterFunc<int, bool> is: bool (out int result)
        TesterFunc<int, bool> tryGetPort = (out int port) =>
        {
            string configValue = Environment.GetEnvironmentVariable("APP_PORT") ?? "8080";
            return int.TryParse(configValue, out port);
        };

        // Build the full fluent chain
        // The Create method returns an ExceptionInvoker<InvalidOperationException, int>
        ExceptionInvoker<InvalidOperationException, int> invoker = new ExceptionCondition<InvalidOperationException>()
            .IsFalse(tryGetPort)
            .Create(port => new InvalidOperationException(
                $"Invalid port number ({port}) configured in APP_PORT environment variable."));

        // TryThrow evaluates tryGetPort; if it returns false (matching IsFalse),
        // the exception is thrown with the out value (0) passed to the handler.
        // If the environment variable is valid, no exception is thrown.
        invoker.TryThrow();

        // You can also create multiple invokers from tester conditions
        TesterFunc<string, bool> tryGetName = (out string name) =>
        {
            name = Environment.GetEnvironmentVariable("APP_NAME") ?? "MyApp";
            return !string.IsNullOrEmpty(name);
        };

        var nameInvoker = new ExceptionCondition<ArgumentNullException>()
            .IsFalse(tryGetName)
            .Create(name => new ArgumentNullException(nameof(name),
                $"APP_NAME resolved to '{name}' which is not a valid application name."));

        nameInvoker.TryThrow();

}
}

```
