---
uid: Cuemon.Extensions.DependencyInjection.ServiceProviderExtensions
example:
- *content
---

The following example wraps the built service provider and then uses `GetServiceDescriptors()` to inspect the registrations that were added to the container.

```csharp
using System;
using System.Linq;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Docs.Samples.DependencyInjection
{
    public static class ServiceProviderExtensionsExample
    {
        public static void Demonstrate()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IClock, SystemClock>();
            services.AddScoped<IJobRepository, InMemoryJobRepository>();

            using var provider = services.BuildServiceProvider();
            var wrappedProvider = new DelegatingServiceProvider(provider);

            var descriptors = wrappedProvider.GetServiceDescriptors().ToList();

            Console.WriteLine(descriptors.Any(descriptor => descriptor.ServiceType == typeof(IClock)));
            Console.WriteLine(descriptors.Any(descriptor => descriptor.ServiceType == typeof(IJobRepository)));
        }

        public interface IClock
        {
        }

        public sealed class SystemClock : IClock
        {
        }

        public interface IJobRepository
        {
        }

        public sealed class InMemoryJobRepository : IJobRepository
        {
        }

        private sealed class DelegatingServiceProvider : IServiceProvider
        {
            private readonly IServiceProvider _provider;

            public DelegatingServiceProvider(IServiceProvider provider)
            {
                _provider = provider;
            }

            public object GetService(Type serviceType)
            {
                return _provider.GetService(serviceType);
            }
        }
    }
}
```
