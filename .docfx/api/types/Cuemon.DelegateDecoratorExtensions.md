---
uid: Cuemon.DelegateDecoratorExtensions
example:
- *content
---

`DelegateDecoratorExtensions` provides extension methods on `Decorator.Enclose` for resolving `MethodInfo` from delegate instances via reflection. This example creates a `Func<int, int, int> add` delegate and an `Action greet` delegate, then calls `ResolveDelegateInfo` to retrieve their underlying `MethodInfo`, with optional fallback to the decorator wrapper when one parameter is null. Key steps include wrapping `null` as a fallback delegate and resolving `MethodInfo` from both the original and wrapped delegates. Console output prints the method name, declaring type name, and static status for each resolved method.

```csharp
using System;
using System.Reflection;
using Cuemon;

namespace MyApp.Reflection
{
    public class DelegateDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // Create a delegate
            Func<int, int, int> add = (a, b) => a + b;

            // Resolve the MethodInfo from the original delegate;
            // the decorator is used as fallback when original is null
            MethodInfo methodInfo = Decorator.Enclose<Delegate>(null, false).ResolveDelegateInfo(add);

            Console.WriteLine($"Method name: {methodInfo.Name}");
            Console.WriteLine($"Declaring type: {methodInfo.DeclaringType?.Name}");
            Console.WriteLine($"Is static: {methodInfo.IsStatic}");

            // When the delegate is not available but the wrapper is:
            Action greet = () => Console.WriteLine("Hello!");
            MethodInfo fromWrapper = Decorator.Enclose<Delegate>(greet).ResolveDelegateInfo(null);
            Console.WriteLine($"\nGreet method: {fromWrapper.Name}");

}}
}

```
