---
uid: Cuemon.Extensions.Reflection.PropertyInfoExtensions
example:
- *content
---

`PropertyInfoExtensions` provides extension methods for `PropertyInfo` to detect auto-implemented properties via `IsAutoProperty`. This example defines a `Sample` class with an auto-property (`AutoProperty`) and a manually implemented property (`ManualProperty`) that uses a backing field with a null-guard in its setter, then retrieves both `PropertyInfo` instances via reflection and calls `IsAutoProperty` on each. Console output displays `True` for the auto-property and `False` for the manual one, confirming the extension correctly distinguishes compiler-generated get/set accessors from custom implementations.

```csharp
// Program.cs
using System;
using System.Reflection;
using Cuemon.Extensions.Reflection;
using MyApp.Examples;

PropertyInfoExtensionsExample.Demonstrate();

namespace MyApp.Examples
{
    public static class PropertyInfoExtensionsExample
    {
        public static void Demonstrate()
        {
            var autoProperty = typeof(Sample).GetProperty(nameof(Sample.AutoProperty), BindingFlags.Instance | BindingFlags.Public);
            var manualProperty = typeof(Sample).GetProperty(nameof(Sample.ManualProperty), BindingFlags.Instance | BindingFlags.Public);

            Console.WriteLine(autoProperty.IsAutoProperty());
            Console.WriteLine(manualProperty.IsAutoProperty());
        }

        private sealed class Sample
        {
            private string _manual;

            public string AutoProperty { get; set; }

            public string ManualProperty
            {
                get => _manual;
                set => _manual = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ee475f1993ba5ca6f3b40dd3"><a href="~/run-actions/ee475f1993ba5ca6f3b40dd3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ee475f1993ba5ca6f3b40dd3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
