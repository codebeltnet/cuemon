---
uid: Cuemon.Extensions.Reflection.PropertyInfoExtensions
example:
- *content
---

The following example demonstrates how to detect whether a property uses an auto-implemented backing field.

```csharp
using System;
using System.Reflection;
using Cuemon.Extensions.Reflection;

namespace MyApp.Examples;

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
```
