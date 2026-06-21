---
uid: Cuemon.Reflection.MemberReflectionOptions
example:
- *content
---

The following example demonstrates how to use <see cref="MemberReflectionOptions" /> when generating a public API inventory for <see cref="DateSpan" />.

```csharp
using System;
using System.Linq;
using System.Reflection;
using Cuemon;
using Cuemon.Reflection;

namespace MyApp.Examples;

public static class MemberReflectionOptionsExample
{
    public static void Demonstrate()
    {
        BindingFlags publicInstanceFlags = MemberReflection.CreateFlags(options =>
        {
            options.ExcludePrivate = true;
            options.ExcludeStatic = true;
        });

        var publicDateSpanMembers = typeof(DateSpan)
            .GetMembers(publicInstanceFlags)
            .Select(member => member.Name)
            .Distinct()
            .OrderBy(name => name)
            .Take(8);

        Console.WriteLine(string.Join(", ", publicDateSpanMembers));

        BindingFlags declaredOnlyFlags = MemberReflection.CreateFlags(options =>
        {
            options.ExcludePrivate = true;
            options.ExcludeStatic = true;
            options.ExcludeInheritancePath = true;
        });

        Console.WriteLine(typeof(DateSpan).GetMethods(publicInstanceFlags).Length);
        Console.WriteLine(typeof(DateSpan).GetMethods(declaredOnlyFlags).Length);

        var documentedOptions = new MemberReflectionOptions
        {
            ExcludePrivate = true,
            ExcludeStatic = true,
            ExcludeInheritancePath = true
        };

        Console.WriteLine($"{documentedOptions.ExcludePrivate}/{documentedOptions.ExcludeStatic}/{documentedOptions.ExcludeInheritancePath}/{documentedOptions.ExcludePublic}");
    }
}
```
