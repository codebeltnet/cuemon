---
uid: Cuemon.ObjectPortrayalOptions
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.ObjectPortrayalOptions"/> with <see cref="Generate.ObjectPortrayal"/> to produce a human-readable property dump of an object.

```csharp
// Program.cs
using System;
using Cuemon;
using Contoso.Diagnostics;

ObjectPortrayalOptionsExample.Run();

namespace Contoso.Diagnostics
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1d623769997244f50b8ff9fd"><a href="~/run-actions/1d623769997244f50b8ff9fd/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1d623769997244f50b8ff9fd/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
