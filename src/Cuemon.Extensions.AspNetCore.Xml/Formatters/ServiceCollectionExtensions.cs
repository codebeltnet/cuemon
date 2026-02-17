using System;
using System.Net.Http;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Xml.Converters;
using Cuemon.Extensions.DependencyInjection;
using Cuemon.Net.Http;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Xml.Formatters
{
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
        /// Adds configuration of <see cref="XmlFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="XmlFormatterOptions"/> in a valid state.
        /// </exception>
        public static IServiceCollection AddXmlFormatterOptions(this IServiceCollection services, Action<XmlFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(services);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            services.TryConfigure(setup ?? (o =>
            {
                o.Settings = options.Settings;
                o.SensitivityDetails = options.SensitivityDetails;
                o.SupportedMediaTypes = options.SupportedMediaTypes;
                o.SynchronizeWithXmlConvert = options.SynchronizeWithXmlConvert;
            }));
            return services;
        }

        /// <summary>
        /// Adds an <see cref="IHttpExceptionDescriptorResponseFormatter"/> that uses <see cref="XmlFormatter"/> as engine of serialization to the specified list of <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null
        /// </exception>
        /// <remarks>Configuration of the <see cref="XmlFormatter"/> is done through a call to <see cref="ServiceProviderServiceExtensions.GetService{T}"/> retrieving an <see cref="IOptions{TOptions}"/> implementation of <see cref="XmlFormatterOptions"/>.</remarks>
        public static IServiceCollection AddXmlExceptionResponseFormatter(this IServiceCollection services, Action<XmlFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(services);
            AddXmlFormatterOptions(services, setup);
            services.TryAddSingleton(provider =>
            {
                var options = provider.GetService<IOptions<XmlFormatterOptions>>().Value;
                var faultDescriptorOptions = provider.GetRequiredService<IOptions<FaultDescriptorOptions>>().Value;
                return new HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>(options)
                    .Adjust(o => o.Settings.Converters.AddHttpExceptionDescriptorConverter(edo => edo.SensitivityDetails = o.SensitivityDetails))
                    .Populate((descriptor, contentType) => new StreamContent(XmlFormatter.SerializeObject(faultDescriptorOptions.FaultDescriptor == PreferredFaultDescriptor.FaultDetails ? descriptor : Decorator.Enclose(descriptor).ToProblemDetails(options.SensitivityDetails), options))
                    {
                        Headers = { { HttpHeaderNames.ContentType, contentType.MediaType } }
                    });
            });
            return services;
        }
    }
}
