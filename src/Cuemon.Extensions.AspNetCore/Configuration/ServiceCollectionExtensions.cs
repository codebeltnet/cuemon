using Cuemon.AspNetCore.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Configuration;
/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds an <see cref="AssemblyCacheBusting"/> service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
    public static IServiceCollection AddAssemblyCacheBusting(this IServiceCollection services)
    {
        return services.AddCacheBusting<AssemblyCacheBusting>();
    }

    /// <summary>
    /// Adds a <see cref="DynamicCacheBusting"/> service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
    public static IServiceCollection AddDynamicCacheBusting(this IServiceCollection services)
    {
        return services.AddCacheBusting<DynamicCacheBusting>();
    }

    /// <summary>
    /// Adds a cache-busting service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
    public static IServiceCollection AddCacheBusting<T>(this IServiceCollection services) where T : class, ICacheBusting
    {
        Validator.ThrowIfNull(services);
        services.AddSingleton<ICacheBusting, T>();
        return services;
    }
}
