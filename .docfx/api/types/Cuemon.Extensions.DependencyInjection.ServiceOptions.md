---
uid: Cuemon.Extensions.DependencyInjection.ServiceOptions
example:
- *content
---

The following example uses `ServiceOptions` to choose the lifetime that is applied when the Cuemon registration helpers add a service to `IServiceCollection`.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Cuemon.Docs.Samples.DependencyInjection;

ServiceOptionsExample.Demonstrate();

namespace Cuemon.Docs.Samples.DependencyInjection
{
    public static class ServiceOptionsExample
    {
        public static void Demonstrate()
        {
            var defaults = new ServiceOptions();
            Console.WriteLine(defaults.Lifetime);

            var registration = new ServiceOptions
            {
                Lifetime = ServiceLifetime.Singleton
            };

            var services = new ServiceCollection();
            services.Add<IMessageWriter, ConsoleMessageWriter>(registration.Lifetime);

            using var provider = services.BuildServiceProvider();

            var first = provider.GetRequiredService<IMessageWriter>();
            var second = provider.GetRequiredService<IMessageWriter>();

            Console.WriteLine(object.ReferenceEquals(first, second));
        }

        public interface IMessageWriter
        {
            void Write(string message);
        }

        public sealed class ConsoleMessageWriter : IMessageWriter
        {
            public void Write(string message)
            {
                Console.WriteLine(message);
            }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f2c381e78849e37218bac586"><a href="~/run-actions/f2c381e78849e37218bac586/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f2c381e78849e37218bac586/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
