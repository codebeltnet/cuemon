using System;
using Cuemon.AspNetCore.Authentication;
using Cuemon.Diagnostics;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Authentication;
/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="MemoryNonceTracker"/> service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
    public static IServiceCollection AddInMemoryDigestAuthenticationNonceTracker(this IServiceCollection services)
    {
        Validator.ThrowIfNull(services);
        services.AddSingleton<INonceTracker, MemoryNonceTracker>();
        return services;
    }

    /// <summary>
    /// Adds a <see cref="AuthorizationResponseHandler"/> service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
    /// <param name="setup">The <see cref="AuthorizationResponseHandlerOptions" /> which may be configured.</param>
    /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="AuthorizationResponseHandlerOptions"/> in a valid state.
    /// </exception>
    public static IServiceCollection AddAuthorizationResponseHandler(this IServiceCollection services, Action<AuthorizationResponseHandlerOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        services.Add<AuthorizationResponseHandler>(o => o.Lifetime = ServiceLifetime.Singleton);
        services.TryConfigure(setup ?? (o =>
        {
            o.CancellationToken = options.CancellationToken;
            o.CancellationTokenProvider = options.CancellationTokenProvider;
            o.FallbackResponseHandler = options.FallbackResponseHandler;
            o.SensitivityDetails = options.SensitivityDetails;
        }));
        services.TryConfigure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = options.SensitivityDetails);
        return services;
    }
}
