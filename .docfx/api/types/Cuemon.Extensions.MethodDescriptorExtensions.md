---
uid: Cuemon.Extensions.MethodDescriptorExtensions
example:
- *content
---

The following example demonstrates how to use MethodDescriptorExtensions to inspect a method's parameter information using the MethodDescriptor API.

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
