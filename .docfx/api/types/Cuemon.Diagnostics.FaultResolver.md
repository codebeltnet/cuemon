---
uid: Cuemon.Diagnostics.FaultResolver
example:
- *content
---

`FaultResolver` registers exception-to-descriptor mappings for structured error reporting, using a validator predicate and a descriptor factory. This example registers a resolver that matches `ArgumentNullException` and produces an `ExceptionDescriptor` with code `"ERR_NULL_ARG"` and a message including the parameter name. It then tests resolution against both a matching `ArgumentNullException("value")` and a non-matching `InvalidOperationException`. Console output shows `True`, `ERR_NULL_ARG`, the descriptive message, the failure message (`"Value cannot be null. (Parameter 'value')"`), and `False` for the unmatched case where the result is `null`.

```csharp
// Program.cs
using System;
using Cuemon.Diagnostics;
using MyApp.Examples;

new FaultResolverExample().Demonstrate();

namespace MyApp.Examples
{
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
            Console.WriteLine(result.Failure.Message); // Value cannot be null. (Parameter 'value')

            // Try to resolve an unrelated exception
            var otherEx = new InvalidOperationException("not handled");
            resolved = resolver.TryResolveFault(otherEx, out result);
            Console.WriteLine(resolved);  // False
            Console.WriteLine(result is null); // True

    }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c6ac16fc8addb4290802f076"><a href="~/run-actions/c6ac16fc8addb4290802f076/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c6ac16fc8addb4290802f076/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
