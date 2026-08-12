using System;
using Cuemon.Extensions.AspNetCore.Xml.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Xml;
/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="XmlFormatterOptions"/> service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="setup">The <see cref="XmlFormatterOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> cannot be null.
    /// </exception>
    public static IServiceCollection AddMinimalXmlOptions(this IServiceCollection services, Action<XmlFormatterOptions> setup = null)
    {
        Validator.ThrowIfNull(services);
        return services.AddXmlExceptionResponseFormatter(setup);
    }
}
