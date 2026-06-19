---
uid: Cuemon.DelegateDecoratorExtensions
example:
- *content
---

The following example shows how to extend `Delegate` with `DelegateDecoratorExtensions` methods to resolve `MethodInfo` from a delegate instance through the decorator pattern.

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
