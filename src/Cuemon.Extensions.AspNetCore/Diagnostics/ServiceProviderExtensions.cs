using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.DependencyInjection;
using Cuemon.Net.Http;

namespace Cuemon.Extensions.AspNetCore.Diagnostics;
/// <summary>
/// Extension methods for the <see cref="IServiceProvider"/> interface.
/// </summary>
public static class ServiceProviderExtensions
{

    /// <summary>
    /// Retrieves a sequence of <see cref="IHttpExceptionDescriptorResponseFormatter"/> services from the specified <paramref name="provider"/>.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider"/> to extend.</param>
    /// <returns>A sequence of <see cref="IHttpExceptionDescriptorResponseFormatter"/> services.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="provider"/> cannot be null.
    /// </exception>
    public static IEnumerable<IHttpExceptionDescriptorResponseFormatter> GetExceptionResponseFormatters(this IServiceProvider provider)
    {
        Validator.ThrowIfNull(provider);
        var descriptors = provider.GetServiceDescriptors().Where(descriptor => descriptor.ServiceType.HasInterfaces(typeof(IHttpExceptionDescriptorResponseFormatter)) &&
                                                                        descriptor.ServiceType.GenericTypeArguments.Length == 1 &&
                                                                        descriptor.ServiceType.GenericTypeArguments[0].HasInterfaces(typeof(IExceptionDescriptorOptions)) &&
                                                                        descriptor.ServiceType.GenericTypeArguments[0].HasInterfaces(typeof(IContentNegotiation)));
        foreach (var descriptor in descriptors)
        {
            if (descriptor.ImplementationInstance != null)
            {
                yield return descriptor.ImplementationInstance as IHttpExceptionDescriptorResponseFormatter;
                continue;
            }

            if (descriptor.ImplementationFactory != null)
            {
                yield return descriptor.ImplementationFactory(provider) as IHttpExceptionDescriptorResponseFormatter;
            }
        }
    }
}
