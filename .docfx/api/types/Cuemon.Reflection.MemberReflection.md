---
uid: Cuemon.Reflection.MemberReflection
example:
- *content
---

The following example demonstrates how to use <see cref="MemberReflection"/> to create custom <see cref="BindingFlags"/> for reflection-based member discovery.

```csharp
using System;
using System.Reflection;
using Cuemon.Reflection; // for MemberReflection, MemberReflectionOptions

namespace MyApp.Examples;

public class MemberReflectionExample
{
    public void Demonstrate()
    {
        // Create flags to find only public instance members (excluding inherited)
        BindingFlags flags = new MemberReflection(
            excludePrivate: true,
            excludeStatic: true,
            excludeInheritancePath: true);
        Console.WriteLine(flags);
        // Output: Instance, Public, DeclaredOnly

        // Use the static CreateFlags factory method
        BindingFlags allFlags = MemberReflection.CreateFlags();
        Console.WriteLine(allFlags);
        // Output: Instance, Static, Public, NonPublic

        // Configure via MemberReflectionOptions
        BindingFlags customFlags = MemberReflection.CreateFlags(o =>
        {
            o.ExcludePrivate = true;
            o.ExcludeStatic = true;
        });

        // Use with reflection
        var members = typeof(string).GetMembers(customFlags);
        Console.WriteLine(members.Length); // Number of public instance members on string

}
}

```
