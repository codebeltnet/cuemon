---
uid: Cuemon.Extensions.MethodDescriptorExtensions
example:
- *content
---

`MethodDescriptorExtensions` provides extension methods for `MethodDescriptor` to inspect parameter information and method metadata. This example retrieves `MethodInfo` for `Console.WriteLine(string)` (which has a parameter) and `Guid.NewGuid()` (which has none), creates `MethodDescriptor` instances via `MethodDescriptor.Create`, then calls `HasParameters()` to check each for parameters. Key steps include resolving the method info via reflection and calling `HasParameters` on each descriptor. Console output confirms `True` for `WriteLine`, `False` for `NewGuid`, and displays the method name and caller name.

```csharp
using System;
using System.Reflection;
using Cuemon.Extensions;
using Cuemon.Reflection;

namespace MyApp.Reflection;

public static class MethodDescriptorExtensionsExample
{
    public static void Demonstrate()
    {
        MethodInfo writeLine = typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(string) });
        MethodInfo newGuid = typeof(Guid).GetMethod(nameof(Guid.NewGuid), BindingFlags.Public | BindingFlags.Static);

        MethodDescriptor withParameters = MethodDescriptor.Create(writeLine);
        MethodDescriptor withoutParameters = MethodDescriptor.Create(newGuid);

        Console.WriteLine(withParameters.HasParameters());
        Console.WriteLine(withoutParameters.HasParameters());
        Console.WriteLine(withParameters.Method.Name);
        Console.WriteLine(withParameters.Caller?.Name);
    }
}

```
