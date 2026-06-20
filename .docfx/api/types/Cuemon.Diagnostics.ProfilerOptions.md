---
uid: Cuemon.Diagnostics.ProfilerOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="ProfilerOptions"/> through a small derived options type.

```csharp
using System;
using System.Reflection;
using Cuemon.Diagnostics;
using Cuemon.Reflection;

namespace MyApp.Examples;

public static class ProfilerOptionsExample
{
    private sealed class SampleProfilerOptions : ProfilerOptions
    {
    }

    public static void Demonstrate()
    {
        ProfilerOptions options = new SampleProfilerOptions
        {
            MethodDescriptor = () => MethodDescriptor.Create(MethodBase.GetCurrentMethod()!).AppendRuntimeArguments(500),
            RuntimeParameters = new object[] { 500 }
        };

        Console.WriteLine(options.MethodDescriptor().MethodName);
        Console.WriteLine(options.MethodDescriptor().RuntimeArguments.Count);
        Console.WriteLine(options.RuntimeParameters.Length);
    }
}

```
