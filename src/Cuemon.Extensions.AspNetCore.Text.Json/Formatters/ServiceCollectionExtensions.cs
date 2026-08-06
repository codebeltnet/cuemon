using System;
using System.Net.Http;
using System.Text.Json;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Json.Converters;
using Cuemon.Extensions.DependencyInjection;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    static ServiceCollectionExtensions()
    {
        Bootstrapper.Initialize();
    }

    /// <summary>
    /// Adds configuration of <see cref="JsonFormatterOptions"/> for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
    /// <param name="setup">The <see cref="JsonFormatterOptions"/> which may be configured.</param>
    /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="JsonFormatterOptions"/> in a valid state.
    /// </exception>
    public static IServiceCollection AddJsonFormatterOptions(this IServiceCollection services, Action<JsonFormatterOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        services.TryConfigure(setup ?? (o =>
        {
            o.Settings = options.Settings;
            o.SensitivityDetails = options.SensitivityDetails;
            o.SupportedMediaTypes = options.SupportedMediaTypes;
        }));
        return services;
    }

    /// <summary>
    /// Adds an <see cref="IHttpExceptionDescriptorResponseFormatter"/> that uses <see cref="JsonFormatter"/> as engine of serialization to the specified list of <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
    /// <param name="setup">The <see cref="JsonFormatterOptions"/> which may be configured.</param>
    /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> cannot be null
    /// </exception>
    /// <remarks>Configuration of the <see cref="JsonFormatter"/> is done through a call to <see cref="ServiceProviderServiceExtensions.GetService{T}"/> retrieving an <see cref="IOptions{TOptions}"/> implementation of <see cref="JsonFormatterOptions"/>.</remarks>
    public static IServiceCollection AddJsonExceptionResponseFormatter(this IServiceCollection services, Action<JsonFormatterOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        AddJsonFormatterOptions(services, setup);
        services.TryAddSingleton(provider =>
        {
            var options = provider.GetService<IOptions<JsonFormatterOptions>>().Value;
            var faultDescriptorOptions = provider.GetRequiredService<IOptions<FaultDescriptorOptions>>().Value;
            return new HttpExceptionDescriptorResponseFormatter<JsonFormatterOptions>(options)
                .Adjust(o =>
                {
                    o.Settings = new JsonSerializerOptions(o.Settings);
                    o.Settings.Converters.AddHttpExceptionDescriptorConverter(edo => edo.SensitivityDetails = o.SensitivityDetails);
                })
                .Populate((descriptor, contentType) => new StreamContent(JsonFormatter.SerializeObject(faultDescriptorOptions.FaultDescriptor == PreferredFaultDescriptor.FaultDetails ? descriptor : Decorator.Enclose(descriptor).ToProblemDetails(options.SensitivityDetails), options))
                {
                    Headers = { { HttpHeaderNames.ContentType, contentType.MediaType } }
                });
        });
        return services;
    }
}
