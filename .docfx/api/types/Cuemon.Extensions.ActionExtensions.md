---
uid: Cuemon.Extensions.ActionExtensions
example:
- *content
---

The following example demonstrates applying the options pattern and factory initialization using the [Configure](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.ActionExtensions.html#Cuemon_Extensions_ActionExtensions_Configure__1_System_Action___0__) and [CreateInstance](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.ActionExtensions.html#Cuemon_Extensions_ActionExtensions_CreateInstance__1_System_Action___0__) extension methods.

```csharp
// Program.cs
using System;
using Cuemon.Configuration;
using Cuemon.Extensions;
using MyApp.Examples;

ActionExtensionsExample.Demonstrate();

namespace MyApp.Examples
{
    public static class ActionExtensionsExample
    {
        private sealed class MyOptions : IParameterObject
        {
            public string Delimiter { get; set; } = ",";

            public string Qualifier { get; set; } = "\"";
        }

        private sealed class MyService
        {
            public string ConnectionString { get; set; } = string.Empty;

            public int Timeout { get; set; } = 30;
        }

        public static void Demonstrate()
        {
            var options = new Action<MyOptions>(setup =>
            {
                setup.Delimiter = ";";
                setup.Qualifier = "'";
            }).Configure();

            var service = new Action<MyService>(factory =>
            {
                factory.ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Docs";
                factory.Timeout = 60;
            }).CreateInstance();

            Console.WriteLine($"{options.Delimiter} {options.Qualifier}");
            Console.WriteLine($"{service.ConnectionString} ({service.Timeout}s)");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="caaa6221b36cc8db6affaba4"><a href="~/run-actions/caaa6221b36cc8db6affaba4/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/caaa6221b36cc8db6affaba4/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
