using System;
using System.Collections.Generic;
using Cuemon.Extensions.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Hosting;
/// <summary>
/// Extension methods for the <see cref="IHostBuilder"/> interface.
/// </summary>
public static class HostBuilderExtensions
{
    /// <summary>
    /// Provides a way to configure the sources of the <see cref="IConfigurationBuilder"/>.
    /// </summary>
    /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to extend.</param>
    /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder.Sources"/> depending on the <see cref="HostBuilderContext.HostingEnvironment"/>.</param>
    /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hostBuilder"/> cannot be null -or-
    /// <paramref name="configureDelegate"/> cannot be null.
    /// </exception>
    public static IHostBuilder ConfigureConfigurationSources(this IHostBuilder hostBuilder, Action<IHostEnvironment, IList<IConfigurationSource>> configureDelegate)
    {
        Validator.ThrowIfNull(hostBuilder);
        Validator.ThrowIfNull(configureDelegate);
        return hostBuilder.ConfigureAppConfiguration((context, builder) =>
        {
            configureDelegate(context.HostingEnvironment, builder.Sources);
        });
    }

    /// <summary>
    /// Provides a way to remove a source of the <see cref="IConfigurationBuilder"/>.
    /// </summary>
    /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to extend.</param>
    /// <param name="predicate">The function delegate that will determine if a source should be removed from the <see cref="IConfigurationBuilder.Sources"/> depending on the <see cref="HostBuilderContext.HostingEnvironment"/>.</param>
    /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
    public static IHostBuilder RemoveConfigurationSource(this IHostBuilder hostBuilder, Func<IHostEnvironment, IConfigurationSource, bool> predicate)
    {
        return ConfigureConfigurationSources(hostBuilder, (environment, sources) =>
        {
            sources.Remove(source => predicate(environment, source));
        });
    }
}
