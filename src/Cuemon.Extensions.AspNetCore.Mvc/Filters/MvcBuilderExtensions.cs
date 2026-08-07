using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters;
/// <summary>
/// Extension methods for the <see cref="IMvcBuilder"/> interface.
/// </summary>
public static class MvcBuilderExtensions
{
    /// <summary>
    /// Registers the specified <paramref name="setup" /> to configure <see cref="ApiKeySentinelOptions"/> in the underlying service collection of <paramref name="builder" />.
    /// </summary>
    /// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
    /// <param name="setup">The <see cref="ApiKeySentinelOptions"/> which may be configured.</param>
    /// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="ApiKeySentinelOptions"/> in a valid state.
    /// </exception>
    public static IMvcBuilder AddApiKeySentinelOptions(this IMvcBuilder builder, Action<ApiKeySentinelOptions> setup = null)
    {
        Validator.ThrowIfNull(builder);
        builder.Services.AddApiKeySentinelOptions(setup);
        return builder;
    }

    /// <summary>
    /// Registers the specified <paramref name="setup" /> to configure <see cref="ThrottlingSentinelOptions"/> in the underlying service collection of <paramref name="builder" />.
    /// </summary>
    /// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
    /// <param name="setup">The <see cref="ThrottlingSentinelOptions"/> which may be configured.</param>
    /// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="ThrottlingSentinelOptions"/> in a valid state.
    /// </exception>
    public static IMvcBuilder AddThrottlingSentinelOptions(this IMvcBuilder builder, Action<ThrottlingSentinelOptions> setup = null)
    {
        Validator.ThrowIfNull(builder);
        builder.Services.AddThrottlingSentinelOptions(setup);
        return builder;
    }

    /// <summary>
    /// Registers the specified <paramref name="setup" /> to configure <see cref="UserAgentSentinelOptions"/> in the underlying service collection of <paramref name="builder" />.
    /// </summary>
    /// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
    /// <param name="setup">The <see cref="UserAgentSentinelOptions"/> which may be configured.</param>
    /// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="UserAgentSentinelOptions"/> in a valid state.
    /// </exception>
    public static IMvcBuilder AddUserAgentSentinelOptions(this IMvcBuilder builder, Action<UserAgentSentinelOptions> setup = null)
    {
        Validator.ThrowIfNull(builder);
        builder.Services.AddUserAgentSentinelOptions(setup);
        return builder;
    }

    /// <summary>
    /// Registers the specified <paramref name="setup" /> to configure <see cref="MvcFaultDescriptorOptions"/> in the underlying service collection of <paramref name="builder" />.
    /// </summary>
    /// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
    /// <param name="setup">The <see cref="MvcFaultDescriptorOptions"/> that may be configured.</param>
    /// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="MvcFaultDescriptorOptions"/> in a valid state.
    /// </exception>
    public static IMvcBuilder AddFaultDescriptorOptions(this IMvcBuilder builder, Action<MvcFaultDescriptorOptions> setup = null)
    {
        Validator.ThrowIfNull(builder);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        builder.Services.TryConfigure(setup ?? (o =>
        {
            o.MarkExceptionHandled = options.MarkExceptionHandled;
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
        builder.Services.TryConfigure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = options.SensitivityDetails);
        return builder;
    }

    /// <summary>
    /// Registers the specified <paramref name="setup" /> to configure <see cref="HttpCacheableOptions"/> in the underlying service collection of <paramref name="builder" />.
    /// </summary>
    /// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
    /// <param name="setup">The <see cref="HttpCacheableOptions"/> which may be configured.</param>
    /// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="HttpCacheableOptions"/> in a valid state.
    /// </exception>
    public static IMvcBuilder AddHttpCacheableOptions(this IMvcBuilder builder, Action<HttpCacheableOptions> setup = null)
    {
        Validator.ThrowIfNull(builder);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        builder.Services.TryConfigure(setup ?? (o =>
        {
            o.CacheControl = options.CacheControl;
            o.Filters = options.Filters;
        }));
        return builder;
    }
}
