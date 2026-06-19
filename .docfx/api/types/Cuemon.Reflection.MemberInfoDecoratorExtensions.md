---
uid: Cuemon.Reflection.MemberInfoDecoratorExtensions
example:
- *content
---

The following example demonstrates how to check whether a MemberInfo has one or more custom attributes using MemberInfoDecoratorExtensions.

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
