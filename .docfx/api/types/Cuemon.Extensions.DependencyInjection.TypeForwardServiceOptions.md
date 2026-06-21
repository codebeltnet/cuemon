---
uid: Cuemon.Extensions.DependencyInjection.TypeForwardServiceOptions
example:
- *content
---

The following example customizes `TypeForwardServiceOptions` so only one nested contract is forwarded when a concrete implementation is added to the dependency-injection container.

```csharp
using System;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Docs.Samples.DependencyInjection
{
    public static class TypeForwardServiceOptionsExample
    {
        public static void Demonstrate()
        {
            var forwarding = new TypeForwardServiceOptions
            {
                Lifetime = ServiceLifetime.Singleton,
                NestedTypeSelector = type => type.GetInterfaces(),
                NestedTypePredicate = type => type == typeof(IMessageHandler)
            };

            Console.WriteLine(forwarding.UseNestedTypeForwarding);
            forwarding.ValidateOptions();

            var services = new ServiceCollection();
            services.Add<MessageDispatcher>(options =>
            {
                options.Lifetime = forwarding.Lifetime;
                options.UseNestedTypeForwarding = forwarding.UseNestedTypeForwarding;
                options.NestedTypeSelector = forwarding.NestedTypeSelector;
                options.NestedTypePredicate = forwarding.NestedTypePredicate;
            });

            using var provider = services.BuildServiceProvider();

            var dispatcher = provider.GetRequiredService<MessageDispatcher>();
            var handler = provider.GetRequiredService<IMessageHandler>();
            var diagnostics = provider.GetService<IDiagnosticSink>();

            Console.WriteLine(object.ReferenceEquals(dispatcher, handler));
            Console.WriteLine(diagnostics is null);
        }

        public interface IMessageHandler
        {
            void Handle(string message);
        }

        public interface IDiagnosticSink
        {
            void Write(string message);
        }

        public sealed class MessageDispatcher : IMessageHandler, IDiagnosticSink
        {
            public void Handle(string message)
            {
                Console.WriteLine(message);
            }

            public void Write(string message)
            {
                Console.WriteLine(message);
            }
        }
    }
}
```
