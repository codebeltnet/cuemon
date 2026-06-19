---
uid: Cuemon.ObjectPortrayalOptions
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.ObjectPortrayalOptions"/> with <see cref="Generate.ObjectPortrayal"/> to produce a human-readable property dump of an object.

```csharp
using System;
using Cuemon;

namespace Contoso.Diagnostics;

public sealed class ObjectPortrayalOptionsExample
{
    public static void Run()
    {
        var options = new ObjectPortrayalOptions
        {
            BypassOverrideCheck = true,
            Delimiter = "; ",
            NullValue = "(none)"
        };

        var profile = new SampleProfile { Name = "Alice", Nickname = null };

        string portrayal = Generate.ObjectPortrayal(profile, setup =>
        {
            setup.BypassOverrideCheck = options.BypassOverrideCheck;
            setup.Delimiter = options.Delimiter;
            setup.NullValue = options.NullValue;
        });

        Console.WriteLine(portrayal);
    }

    private sealed class SampleProfile
    {
        public string Name { get; set; }

        public string Nickname { get; set; }
    }
}
```
