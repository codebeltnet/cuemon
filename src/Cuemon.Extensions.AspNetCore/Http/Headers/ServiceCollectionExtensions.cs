using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Http.Headers;
/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds configuration of <see cref="ApiKeySentinelOptions"/> for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
    /// <param name="setup">The <see cref="ApiKeySentinelOptions"/> which may be configured.</param>
    /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="ApiKeySentinelOptions"/> in a valid state.
    /// </exception>
    public static IServiceCollection AddApiKeySentinelOptions(this IServiceCollection services, Action<ApiKeySentinelOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        services.TryConfigure(setup ?? (o =>
        {
            o.AllowedKeys = options.AllowedKeys;
            o.ForbiddenMessage = options.ForbiddenMessage;
            o.GenericClientMessage = options.GenericClientMessage;
            o.GenericClientStatusCode = options.GenericClientStatusCode;
            o.HeaderName = options.HeaderName;
            o.ResponseHandler = options.ResponseHandler;
            o.UseGenericResponse = options.UseGenericResponse;
        }));
        return services;
    }

    /// <summary>
    /// Adds configuration of <see cref="UserAgentSentinelOptions"/> for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
    /// <param name="setup">The <see cref="UserAgentSentinelOptions"/> which may be configured.</param>
    /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="UserAgentSentinelOptions"/> in a valid state.
    /// </exception>
    public static IServiceCollection AddUserAgentSentinelOptions(this IServiceCollection services, Action<UserAgentSentinelOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        services.TryConfigure(setup ?? (o =>
        {
            o.AllowedUserAgents = options.AllowedUserAgents;
            o.BadRequestMessage = options.BadRequestMessage;
            o.ForbiddenMessage = options.ForbiddenMessage;
            o.RequireUserAgentHeader = options.RequireUserAgentHeader;
            o.ResponseHandler = options.ResponseHandler;
            o.UseGenericResponse = options.UseGenericResponse;
            o.ValidateUserAgentHeader = options.ValidateUserAgentHeader;
        }));
        return services;
    }
}
