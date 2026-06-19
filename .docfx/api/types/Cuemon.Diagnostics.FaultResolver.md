---
uid: Cuemon.Diagnostics.FaultResolver
example:
- *content
---

The following example demonstrates how to use <see cref="FaultResolver"/> to register exception-to-descriptor mappings for structured error reporting.

```csharp
using System;
using Cuemon.Diagnostics; // for FaultResolver, ExceptionDescriptor

namespace MyApp.Examples;

public class FaultResolverExample
{
    public void Demonstrate()
    {
        // Register a resolver that handles ArgumentNullException
        var resolver = new FaultResolver(
            validator: ex => ex is ArgumentNullException,
            descriptor: ex =>
            {
                var argEx = (ArgumentNullException)ex;
                return new ExceptionDescriptor(
                    argEx,
                    "ERR_NULL_ARG",
                    $"The parameter '{argEx.ParamName}' cannot be null.");
            });

        // Try to resolve an ArgumentNullException
        var testEx = new ArgumentNullException("value");
        bool resolved = resolver.TryResolveFault(testEx, out ExceptionDescriptor result);

        Console.WriteLine(resolved);               // True
        Console.WriteLine(result.Code);            // ERR_NULL_ARG
        Console.WriteLine(result.Message);         // The parameter 'value' cannot be null.
        Console.WriteLine(result.Failure.Message); // value

        // Try to resolve an unrelated exception
        var otherEx = new InvalidOperationException("not handled");
        resolved = resolver.TryResolveFault(otherEx, out result);
        Console.WriteLine(resolved);  // False
        Console.WriteLine(result is null); // True

}
}

```
