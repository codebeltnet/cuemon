using System;

namespace Asp.Versioning.Builder;

internal static class EndpointBuilderFinalizer
{
    internal sealed class InjectApiVersion : IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public InjectApiVersion(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }
}
