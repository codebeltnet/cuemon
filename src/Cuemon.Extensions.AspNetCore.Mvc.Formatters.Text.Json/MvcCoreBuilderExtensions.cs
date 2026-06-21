using System;
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json
{
    /// <summary>
    /// Extension methods for the <see cref="IMvcCoreBuilder"/> interface.
    /// </summary>
    public static class MvcCoreBuilderExtensions
    {
        /// <summary>
        /// Adds the JSON serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcCoreBuilder"/>.</param>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null.
        /// </exception>
        public static IMvcCoreBuilder AddJsonFormatters(this IMvcCoreBuilder builder, Action<JsonFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(builder);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, JsonSerializationMvcOptionsSetup>());
            AddJsonFormattersOptions(builder, setup);
            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="JsonFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="JsonFormatterOptions"/> in a valid state.
        /// </exception>
        public static IMvcCoreBuilder AddJsonFormattersOptions(this IMvcCoreBuilder builder, Action<JsonFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(builder);
            builder.Services.AddJsonExceptionResponseFormatter(setup);
            return builder;
        }
    }
}
