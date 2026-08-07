using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Cuemon.Extensions.AspNetCore.Diagnostics;
/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="ServerTiming"/> service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="setup">The <see cref="ServerTimingOptions"/> that may be configured.</param>
    /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
    public static IServiceCollection AddServerTiming(this IServiceCollection services, Action<ServerTimingOptions> setup = null)
    {
        return services.AddServerTiming<ServerTiming>(setup);
    }

    /// <summary>
    /// Adds an implementation of <see cref="IServerTiming"/> service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="setup">The <see cref="ServerTimingOptions"/> that may be configured.</param>
    /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
    public static IServiceCollection AddServerTiming<T>(this IServiceCollection services, Action<ServerTimingOptions> setup = null) where T : class, IServerTiming
    {
        Validator.ThrowIfNull(services);
        services.TryAddScoped<IServerTiming, T>();
        services.AddServerTimingOptions(setup);
        return services;
    }

    /// <summary>
    /// Registers the specified <paramref name="setup" /> to configure <see cref="ServerTimingOptions"/> in the <paramref name="services"/> collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
    /// <param name="setup">The <see cref="ServerTimingOptions"/> that may be configured.</param>
    /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="ServerTimingOptions"/> in a valid state.
    /// </exception>
    public static IServiceCollection AddServerTimingOptions(this IServiceCollection services, Action<ServerTimingOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        services.TryConfigure(setup ?? (o =>
        {
            o.LogLevelSelector = options.LogLevelSelector;
            o.SuppressHeaderPredicate = options.SuppressHeaderPredicate;
            o.MethodDescriptor = options.MethodDescriptor;
            o.RuntimeParameters = options.RuntimeParameters;
            o.TimeMeasureCompletedThreshold = options.TimeMeasureCompletedThreshold;
        }));
        return services;
    }

    /// <summary>
    /// Registers the specified <paramref name="setup" /> to configure <see cref="FaultDescriptorOptions"/> in the <paramref name="services"/> collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
    /// <param name="setup">The <see cref="FaultDescriptorOptions"/> that may be configured.</param>
    /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="FaultDescriptorOptions"/> in a valid state.
    /// </exception>
    public static IServiceCollection AddFaultDescriptorOptions(this IServiceCollection services, Action<FaultDescriptorOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        services.TryConfigure(setup ?? (o =>
        {
            o.SensitivityDetails = options.SensitivityDetails;
            o.ExceptionCallback = options.ExceptionCallback;
            o.ExceptionDescriptorResolver = options.ExceptionDescriptorResolver;
            o.HttpFaultResolvers = options.HttpFaultResolvers;
            o.RequestEvidenceProvider = options.RequestEvidenceProvider;
            o.RootHelpLink = options.RootHelpLink;
            o.UseBaseException = options.UseBaseException;
            o.CancellationToken = options.CancellationToken;
            o.CancellationTokenProvider = options.CancellationTokenProvider;
        }));
        services.TryConfigure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = options.SensitivityDetails);
        return services;
    }

    /// <summary>
    /// Registers the specified <paramref name="setup" /> to configure <see cref="ExceptionDescriptorOptions"/> in the <paramref name="services"/> collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
    /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> that may be configured.</param>
    /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
    public static IServiceCollection AddExceptionDescriptorOptions(this IServiceCollection services, Action<ExceptionDescriptorOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        services.TryConfigure(setup ?? (o =>
        {
            o.SensitivityDetails = options.SensitivityDetails;
        }));
        services.TryConfigure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = options.SensitivityDetails);
        return services;
    }

    /// <summary>
    /// Registers an action used to post-configure all instances of <see cref="IExceptionDescriptorOptions"/> in the <paramref name="services"/> collection.
    /// These are run after <see cref="OptionsServiceCollectionExtensions.Configure{TOptions}(IServiceCollection,Action{TOptions})"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to extend.</param>
    /// <param name="setup">The <see cref="IExceptionDescriptorOptions"/> which need to be configured.</param>
    /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
    public static IServiceCollection PostConfigureAllExceptionDescriptorOptions(this IServiceCollection services, Action<IExceptionDescriptorOptions> setup)
    {
        Validator.ThrowIfNull(services);
        services.PostConfigureAllOf(setup);
        return services;
    }
}
