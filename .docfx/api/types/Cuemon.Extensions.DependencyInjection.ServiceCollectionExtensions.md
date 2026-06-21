---
uid: Cuemon.Extensions.DependencyInjection.ServiceCollectionExtensions
example:
- *content
---

`ServiceCollectionExtensions` provides registration methods for `IServiceCollection` that support multi-contract resolution, typed options, and bulk post-configuration. This example defines an `OrdersMessageHandler` implementing both `IMessageHandler<OrdersChannel>` and `IDependencyInjectionMarker<OrdersChannel>`, then registers it with various lifecycle options using `Add`, `TryAdd`, and `TryConfigure` overloads including scoped and singleton lifetimes. It also demonstrates `PostConfigureAllOf<HandlerOptions>` for bulk configuration of options instances. After building the service provider and creating a scope, the concrete handler, typed contract, and marker are resolved and compared by reference. Console output confirms that all three resolve to the same instance and that `HandlerOptions.Label` is correctly set to `"post-configured"`.

```csharp
using System;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cuemon.Docs.Samples.DependencyInjection
{
    public static class ServiceCollectionExtensionsExample
    {
        public static void Demonstrate()
        {
            var services = new ServiceCollection();

            services.Add<OrdersMessageHandler>(options =>
            {
                options.Lifetime = ServiceLifetime.Scoped;
            });
            services.TryAdd<OrdersMessageHandler>(options =>
            {
                options.Lifetime = ServiceLifetime.Scoped;
            });
            services.TryAdd<IMessageHandler<OrdersChannel>, OrdersMessageHandler>(options =>
            {
                options.Lifetime = ServiceLifetime.Singleton;
            });
            services.TryAdd(typeof(IMessageHandler<OrdersChannel>), typeof(OrdersMessageHandler), options =>
            {
                options.Lifetime = ServiceLifetime.Singleton;
            });
            services.TryAdd<HandlerOptions>(typeof(IMessageHandler<OrdersChannel>), typeof(OrdersMessageHandler), ServiceLifetime.Scoped, options =>
            {
                options.Label = "typed";
            });
            services.TryAdd<IMessageHandler<OrdersChannel>, OrdersMessageHandler, HandlerOptions>(ServiceLifetime.Scoped, options =>
            {
                options.Enabled = true;
            });
            services.TryConfigure<HandlerOptions>(options =>
            {
                options.Label = "configured";
            });
            services.PostConfigureAllOf<HandlerOptions>(options =>
            {
                options.Label = "post-configured";
            });

            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();

            var concrete = scope.ServiceProvider.GetRequiredService<OrdersMessageHandler>();
            var typedContract = scope.ServiceProvider.GetRequiredService<IMessageHandler<OrdersChannel>>();
            var marker = scope.ServiceProvider.GetRequiredService<IDependencyInjectionMarker<OrdersChannel>>();
            var handlerOptions = scope.ServiceProvider.GetRequiredService<IOptions<HandlerOptions>>().Value;

            Console.WriteLine(object.ReferenceEquals(concrete, typedContract));
            Console.WriteLine(object.ReferenceEquals(concrete, marker));
            Console.WriteLine(typedContract.Name);
            Console.WriteLine(handlerOptions.Label);
        }

        public sealed class OrdersChannel
        {
        }

        public interface IMessageHandler
        {
            string Name { get; }
        }

        public interface IMessageHandler<TChannel> : IMessageHandler, IDependencyInjectionMarker<TChannel>
        {
        }

        public sealed class HandlerOptions
        {
            public bool Enabled { get; set; }

            public string Label { get; set; } = string.Empty;
        }

        public sealed class OrdersMessageHandler : IMessageHandler<OrdersChannel>
        {
            public string Name => nameof(OrdersMessageHandler);
        }
    }
}
```
