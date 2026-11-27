---
uid: Cuemon.ExceptionCondition`1
example:
- *content
---

The following example demonstrates how to use <see cref="ExceptionCondition{TException}"/> to fluently define a condition under which a specific exception should be thrown. The sample builds conditions for both true and false outcomes, calls TryThrow, and prints the exception messages so the resulting control flow is visible.

```csharp
// Program.cs
using System;
using Cuemon;
using MyApp.Examples;

new ExceptionConditionExample().Demonstrate();

namespace MyApp.Examples
{
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
            try
            {
                invoker.TryThrow();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Throw ArgumentException only when a condition is false
            var falseInvoker = new ExceptionCondition<ArgumentException>()
                .IsFalse(() => Environment.UserName == "admin")
                .Create(() => new ArgumentException("Only admin can call this method."));

            try
            {
                falseInvoker.TryThrow();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

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
            try
            {
                dataInvoker.TryThrow();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

    }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ea8ea38a981d8329f9c1acc7"><a href="~/run-actions/ea8ea38a981d8329f9c1acc7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ea8ea38a981d8329f9c1acc7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
