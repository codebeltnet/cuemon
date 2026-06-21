---
uid: Cuemon.ExceptionCondition`1
example:
- *content
---

The following example demonstrates how to use <see cref="ExceptionCondition{TException}"/> to fluently define a condition under which a specific exception should be thrown.

```csharp
using System;
using Cuemon; // for ExceptionCondition

namespace MyApp.Examples;

public class ExceptionConditionExample
{
    public void Demonstrate()
    {
        // Throw InvalidOperationException only when a condition is true
        var invoker = new ExceptionCondition<InvalidOperationException>()
            .IsTrue(() => DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            .Create(() => new InvalidOperationException("Cannot run this operation on Mondays."));

        // If today is Monday, TryThrow will throw InvalidOperationException
        // If today is not Monday, TryThrow does nothing
        invoker.TryThrow();

        // Throw ArgumentException only when a condition is false
        var falseInvoker = new ExceptionCondition<ArgumentException>()
            .IsFalse(() => Environment.UserName == "admin")
            .Create(() => new ArgumentException("Only admin can call this method."));

        falseInvoker.TryThrow();

        // Use the TesterFunc overload to pass data to the exception
        TesterFunc<string, bool> tryGetValue = (out string value) =>
        {
            value = "cached-data";
            return true; // data exists
        };

        var dataInvoker = new ExceptionCondition<InvalidOperationException>()
            .IsTrue(tryGetValue)
            .Create(data => new InvalidOperationException(
                $"Value '{data}' has expired. Please refresh."));

        // Since condition returns true, this throws
        dataInvoker.TryThrow();

}
}

```
