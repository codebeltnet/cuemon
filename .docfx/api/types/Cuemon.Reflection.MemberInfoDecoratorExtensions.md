---
uid: Cuemon.Reflection.MemberInfoDecoratorExtensions
example:
- *content
---

`MemberInfoDecoratorExtensions` provides extension methods on `Decorator.Enclose` for checking whether a `MemberInfo` has one or more custom attributes via `HasAttribute`. This example gets `MemberInfo` for a method without attributes and another decorated with `[Obsolete]`, then calls `HasAttribute` with single (`ObsoleteAttribute`) and multiple attribute types on each. Console output shows `False` for the method without `ObsoleteAttribute`, `False` for `Obsolete` or `EditorBrowsable`, and `True` for the deprecated method, confirming correct attribute detection through the decorator pattern.

```csharp
using System;
using System.ComponentModel;
using System.Reflection;
using Cuemon;
using Cuemon.Reflection;

namespace MyApp.Reflection
{
    public class MemberInfoDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // Get a MemberInfo for a method
            MemberInfo demoMethod = typeof(MemberInfoDecoratorExtensionsExample)
                .GetMethod(nameof(Demonstrate));

            // Check if the method has a specific attribute
            bool hasObsolete = Decorator.Enclose(demoMethod)
                .HasAttribute(typeof(ObsoleteAttribute));
            Console.WriteLine($"Has ObsoleteAttribute: {hasObsolete}"); // False

            // Check for multiple attributes at once
            bool hasAny = Decorator.Enclose(demoMethod)
                .HasAttribute(typeof(ObsoleteAttribute), typeof(EditorBrowsableAttribute));
            Console.WriteLine($"Has Obsolete or EditorBrowsable: {hasAny}"); // False

            // A member with an attribute
            MemberInfo deprecatedMethod = typeof(MemberInfoDecoratorExtensionsExample)
                .GetMethod(nameof(OldMethod));

            bool isDeprecated = Decorator.Enclose(deprecatedMethod)
                .HasAttribute(typeof(ObsoleteAttribute));
            Console.WriteLine($"OldMethod has ObsoleteAttribute: {isDeprecated}"); // True
        }

        [Obsolete("Use Demonstrate instead.")]
        public void OldMethod()
        {
        }
    }
}
```
