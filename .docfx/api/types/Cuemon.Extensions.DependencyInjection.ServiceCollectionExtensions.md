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

Configure delivery retries with `AddConfiguredOptions<DeliveryOptions>` to let Microsoft Options construct the Parameter Object, calculate its retry budget during post-configuration, and validate the final settings. This example adds an ordinary Microsoft configurator that raises the attempt limit to four, then resolves the cached default options directly and through `IOptions<DeliveryOptions>`. The output shows a 15-second retry budget and a shared default instance. The injectable `Action<DeliveryOptions>` is the exact primary delegate and sets three attempts on a fresh object; invoking it directly does not calculate the budget or validate the object. Further `AddConfiguredOptions<DeliveryOptions>` calls would be ignored, while ordinary `Configure` calls still compose. Cuemon's conventions participate for all options names in registration order, and recoverable validation failures surface as `OptionsValidationException` when Microsoft materializes options.

```csharp
using System;
using Cuemon.Configuration;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Delivery.Configuration;

public static class DeliveryApplication
{
    public static void Main()
    {
        Action<DeliveryOptions> setup = options =>
        {
            options.MaxAttempts = 3;
            options.RetryDelay = TimeSpan.FromSeconds(5);
        };
        var services = new ServiceCollection();
        services.AddConfiguredOptions(setup);
        services.Configure<DeliveryOptions>(options => options.MaxAttempts = 4);

        using (var provider = services.BuildServiceProvider())
        {
            var delivery = provider.GetRequiredService<DeliveryOptions>();
            var microsoftOptions = provider.GetRequiredService<IOptions<DeliveryOptions>>().Value;
            Console.WriteLine($"Attempts: {delivery.MaxAttempts}; retry budget: {delivery.RetryBudget.TotalSeconds} seconds");
            Console.WriteLine($"Shared default instance: {ReferenceEquals(delivery, microsoftOptions)}");

            var configure = provider.GetRequiredService<Action<DeliveryOptions>>();
            var fresh = new DeliveryOptions();
            configure(fresh);
            Console.WriteLine($"Primary configurator attempts: {fresh.MaxAttempts}");
        }
    }
}

public sealed class DeliveryOptions : IPostConfigurableParameterObject, IValidatableParameterObject
{
    public int MaxAttempts { get; set; }

    public TimeSpan RetryDelay { get; set; }

    public TimeSpan RetryBudget { get; private set; }

    public void PostConfigureOptions()
    {
        RetryBudget = TimeSpan.FromTicks(RetryDelay.Ticks * (MaxAttempts - 1));
    }

    public void ValidateOptions()
    {
        if (MaxAttempts < 1 || RetryDelay < TimeSpan.Zero)
        {
            throw new InvalidOperationException("Delivery requires at least one attempt and a nonnegative retry delay.");
        }
    }
}
```
