using System;
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Text.Json;
/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="JsonFormatterOptions"/> service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="setup">The <see cref="JsonFormatterOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
    /// <remarks>
    /// This method registers a <see cref="MinimalJsonOptions"/> configuration as a singleton <see cref="IConfigureOptions{TOptions}"/> for <see cref="JsonOptions"/>
    /// and delegates to <see cref="Formatters.ServiceCollectionExtensions.AddJsonExceptionResponseFormatter"/> to configure the JSON exception response formatter.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> cannot be null.
    /// </exception>
    public static IServiceCollection AddMinimalJsonOptions(this IServiceCollection services, Action<JsonFormatterOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<JsonOptions>, MinimalJsonOptions>());
        return services.AddJsonExceptionResponseFormatter(setup);
    }
}
