---
uid: Cuemon.Extensions.Reflection.MemberInfoExtensions
example:
- *content
---

`MemberInfoExtensions` provides extension methods for `MemberInfo` to check for the presence of one or more custom attributes via `HasAttributes`. This example defines a class with an `[Description]`-annotated property, an `[Obsolete]`-annotated field, and a regular property with no attributes, then iterates all public instance members calling `HasAttributes` with `DescriptionAttribute` and `ObsoleteAttribute` types. Console output shows each member name, its `MemberType`, and whether it carries any of the target attributes, demonstrating attribute-based filtering or validation of reflected members.

```csharp
using System;
using System.ComponentModel;
using System.Reflection;
using Cuemon.Extensions.Reflection;

namespace MyApp.Examples;

public class MemberInfoExtensionsExample
{
    [Description("Sample property with a DescriptionAttribute")]
    public string AnnotatedProperty { get; set; }

    [Obsolete("This field is obsolete.")]
    public string ObsoleteField;

    public string RegularProperty { get; set; }

    public static void Main()
    {
        var example = new MemberInfoExtensionsExample();
        var members = typeof(MemberInfoExtensionsExample).GetMembers(BindingFlags.Instance | BindingFlags.Public);

        foreach (MemberInfo member in members)
        {
            bool hasAttributes = member.HasAttributes(typeof(DescriptionAttribute), typeof(ObsoleteAttribute));
            Console.WriteLine($"{member.Name} ({member.MemberType}): has target attributes = {hasAttributes}");

}}
}

```
