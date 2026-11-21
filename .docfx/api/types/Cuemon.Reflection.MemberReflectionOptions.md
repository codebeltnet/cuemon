---
uid: Cuemon.Reflection.MemberReflectionOptions
example:
- *content
---

The following example demonstrates how to use <see cref="MemberReflectionOptions" /> when generating a public API inventory for <see cref="DateSpan" />.

```csharp
// Program.cs
using System;
using System.Linq;
using System.Reflection;
using Cuemon;
using Cuemon.Reflection;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2cae07aa3e75c422e0f7696d"><a href="~/run-actions/2cae07aa3e75c422e0f7696d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2cae07aa3e75c422e0f7696d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
