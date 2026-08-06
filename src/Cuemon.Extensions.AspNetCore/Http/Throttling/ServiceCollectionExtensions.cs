using System;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Http.Throttling;
/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="MemoryThrottlingCache"/> service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
    public static IServiceCollection AddMemoryThrottlingCache(this IServiceCollection services)
    {
        return services.AddThrottlingCache<MemoryThrottlingCache>();
    }

    /// <summary>
    /// Adds a throttling cache service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
    public static IServiceCollection AddThrottlingCache<T>(this IServiceCollection services) where T : class, IThrottlingCache
    {
        Validator.ThrowIfNull(services);
        services.AddSingleton<IThrottlingCache, T>();
        return services;
    }

    /// <summary>
    /// Adds configuration of <see cref="ThrottlingSentinelOptions"/> for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
    /// <param name="setup">The <see cref="ThrottlingSentinelOptions"/> which may be configured.</param>
    /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="ThrottlingSentinelOptions"/> in a valid state.
    /// </exception>
    public static IServiceCollection AddThrottlingSentinelOptions(this IServiceCollection services, Action<ThrottlingSentinelOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        services.TryConfigure(setup ?? (o =>
        {
            o.ContextResolver = options.ContextResolver;
            o.Quota = options.Quota;
            o.RateLimitHeaderName = options.RateLimitHeaderName;
            o.RateLimitRemainingHeaderName = options.RateLimitRemainingHeaderName;
            o.RateLimitResetHeaderName = options.RateLimitResetHeaderName;
            o.RateLimitResetScope = options.RateLimitResetScope;
            o.ResponseHandler = options.ResponseHandler;
            o.RetryAfterScope = options.RetryAfterScope;
            o.TooManyRequestsMessage = options.TooManyRequestsMessage;
            o.UseRetryAfterHeader = options.UseRetryAfterHeader;
        }));
        return services;
    }
}
