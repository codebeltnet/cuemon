using System;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.DependencyInjection;
public class ServiceProviderExtensionsTest : Test
{
    public ServiceProviderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetServiceDescriptors_ShouldGetDescriptors_WhenProviderWrapsServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton(new object());

        var serviceProvider = services.BuildServiceProvider();
        var wrappedProvider = new DelegatingServiceProvider(serviceProvider);

        var descriptors = wrappedProvider.GetServiceDescriptors().ToList();

        Assert.Contains(descriptors, descriptor => descriptor.ServiceType == typeof(object));
    }

    [Fact]
    public void GetServiceDescriptors_ShouldGetDescriptors_WhenProviderIsWrappedByAspVersioningInjectApiVersion()
    {
        var services = new ServiceCollection();
        services.AddSingleton(new object());

        var serviceProvider = services.BuildServiceProvider();
        var wrappedProvider = new Asp.Versioning.Builder.EndpointBuilderFinalizer.InjectApiVersion(serviceProvider);

        var descriptors = wrappedProvider.GetServiceDescriptors().ToList();

        Assert.EndsWith("Asp.Versioning.Builder.EndpointBuilderFinalizer+InjectApiVersion", wrappedProvider.GetType().FullName, StringComparison.Ordinal);
        Assert.Contains(descriptors, descriptor => descriptor.ServiceType == typeof(object));
    }

    [Fact]
    public void GetServiceDescriptors_ShouldThrowNotSupportedExceptionWithUnsupportedProviderMessage_WhenProviderWrapsMultipleServiceProviders()
    {
        var services = new ServiceCollection();
        var primaryProvider = services.BuildServiceProvider();
        var secondaryProvider = services.BuildServiceProvider();
        var wrappedProvider = new AmbiguousDelegatingServiceProvider(primaryProvider, secondaryProvider);

        var exception = Assert.Throws<NotSupportedException>(() => wrappedProvider.GetServiceDescriptors().ToList());

        Assert.Contains("This method does not support", exception.Message, StringComparison.Ordinal);
        Assert.Contains(typeof(AmbiguousDelegatingServiceProvider).FullName, exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void GetServiceDescriptors_ShouldThrowNotSupportedExceptionWithCycleMessage_WhenProviderGraphIsCyclic()
    {
        var primaryProvider = new CyclicDelegatingServiceProvider();
        var secondaryProvider = new CyclicDelegatingServiceProvider();
        primaryProvider.Provider = secondaryProvider;
        secondaryProvider.Provider = primaryProvider;

        var exception = Assert.Throws<NotSupportedException>(() => primaryProvider.GetServiceDescriptors().ToList());

        Assert.Contains("cyclic IServiceProvider graph", exception.Message, StringComparison.Ordinal);
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

    private sealed class CyclicDelegatingServiceProvider : IServiceProvider
    {
        public IServiceProvider Provider { get; set; }

        public object GetService(Type serviceType)
        {
            return Provider.GetService(serviceType);
        }
    }

    private sealed class AmbiguousDelegatingServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _primaryProvider;
        private readonly IServiceProvider _secondaryProvider;

        public AmbiguousDelegatingServiceProvider(IServiceProvider primaryProvider, IServiceProvider secondaryProvider)
        {
            _primaryProvider = primaryProvider;
            _secondaryProvider = secondaryProvider;
        }

        public object GetService(Type serviceType)
        {
            return _primaryProvider.GetService(serviceType) ?? _secondaryProvider.GetService(serviceType);
        }
    }
}
