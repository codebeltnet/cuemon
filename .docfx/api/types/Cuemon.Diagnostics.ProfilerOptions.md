---
uid: Cuemon.Diagnostics.ProfilerOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="ProfilerOptions"/> through a small derived options type.

```csharp
// Program.cs
using System;
using System.Reflection;
using Cuemon.Diagnostics;
using Cuemon.Reflection;
using MyApp.Examples;

ProfilerOptionsExample.Demonstrate();

namespace MyApp.Examples
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="13c72b068bcf7ef5096f795e"><a href="~/run-actions/13c72b068bcf7ef5096f795e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/13c72b068bcf7ef5096f795e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
