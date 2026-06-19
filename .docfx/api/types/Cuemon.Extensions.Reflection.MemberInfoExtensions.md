---
uid: Cuemon.Extensions.Reflection.MemberInfoExtensions
example:
- *content
---

The following example demonstrates checking whether a member has specific custom attributes using the <xref:Cuemon.Extensions.Reflection.MemberInfoExtensions.HasAttributes(System.Reflection.MemberInfo,System.Type[])> extension method.

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
