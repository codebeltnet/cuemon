using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceProvider"/> interface.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets an enumeration of ALL <see cref="ServiceDescriptor"/> instances from the specified <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> to extend.</param>
        /// <returns>An enumeration of ALL <see cref="ServiceDescriptor"/> instances from the specified <paramref name="provider"/>.</returns>
        /// <exception cref="NotSupportedException">This method does not support {providerType.FullName}.</exception>
        public static IEnumerable<ServiceDescriptor> GetServiceDescriptors(this IServiceProvider provider)
        {
            Validator.ThrowIfNull(provider);
            var providerType = provider.GetType();
            var visitedProviders = new List<IServiceProvider> { provider };

            while (true)
            {
                var callSiteFactory = Decorator.Enclose(providerType).GetAllProperties().SingleOrDefault(pi => pi.Name == "CallSiteFactory")?.GetValue(provider);
                if (callSiteFactory != null)
                {
                    var callSiteFactoryType = callSiteFactory.GetType();
                    return Decorator.Enclose(callSiteFactoryType).GetAllProperties().SingleOrDefault(pi => pi.Name == "Descriptors")?.GetValue(callSiteFactory) as IEnumerable<ServiceDescriptor>;
                }

                if (!TryLocateEmbeddedServiceProvider(provider, providerType, out var embeddedProvider)) { break; }
                if (visitedProviders.Any(visitedProvider => ReferenceEquals(visitedProvider, embeddedProvider.ServiceProvider))) { break; }
                provider = embeddedProvider.ServiceProvider;
                providerType = embeddedProvider.ProviderType;
                visitedProviders.Add(provider);
            }

            throw new NotSupportedException($"This method does not support {providerType.FullName}.");
        }

        private static bool TryLocateEmbeddedServiceProvider(IServiceProvider originatingProvider, Type originatingProviderType, out (IServiceProvider ServiceProvider, Type ProviderType) embeddedProvider)
        {
            var nestedProviders = Decorator.Enclose(originatingProviderType).GetAllFields()
                .Where(fi => !fi.IsStatic && typeof(IServiceProvider).IsAssignableFrom(fi.FieldType))
                .Select(fi => new { fi.Name, Provider = fi.GetValue(originatingProvider) as IServiceProvider })
                .Where(candidate => candidate.Provider != null && !ReferenceEquals(candidate.Provider, originatingProvider))
                .ToList();

            if (originatingProviderType.Name == "ServiceProviderEngineScope")
            {
                var rootProvider = Decorator.Enclose(originatingProviderType).GetAllProperties().SingleOrDefault(pi => pi.Name == "RootProvider")?.GetValue(originatingProvider) as IServiceProvider;
                rootProvider ??= nestedProviders.SingleOrDefault(candidate => candidate.Name.IndexOf("RootProvider", StringComparison.OrdinalIgnoreCase) >= 0)?.Provider;
                rootProvider ??= nestedProviders.Count == 1 ? nestedProviders[0].Provider : null;

                if (rootProvider != null)
                {
                    embeddedProvider = new ValueTuple<IServiceProvider, Type>(rootProvider, rootProvider.GetType());
                    return true;
                }
            }

            var providers = nestedProviders.Select(candidate => candidate.Provider)
                .Distinct()
                .ToList();

            if (providers.Count == 1)
            {
                var nestedProvider = providers[0];
                embeddedProvider = new ValueTuple<IServiceProvider, Type>(nestedProvider, nestedProvider.GetType());
                return true;
            }
            embeddedProvider = default;
            return false;
        }
    }
}
