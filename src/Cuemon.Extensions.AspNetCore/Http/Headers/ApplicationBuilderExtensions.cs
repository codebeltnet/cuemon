using System;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Builder;

namespace Cuemon.Extensions.AspNetCore.Http.Headers
{
    /// <summary>
    /// Extension methods for the <see cref="IApplicationBuilder"/> interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a correlation identifier HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="CorrelationIdentifierOptions"/> middleware which may be configured.</param>
        /// <returns>An <see cref="IApplicationBuilder"/> that can be used to further configure the request pipeline.</returns>
        /// <remarks>Default HTTP header name is <c>X-Correlation-ID</c>.</remarks>
        public static IApplicationBuilder UseCorrelationIdentifier(this IApplicationBuilder builder, Action<CorrelationIdentifierOptions> setup = null)
        {
            return MiddlewareBuilderFactory.UseConfigurableMiddleware<CorrelationIdentifierMiddleware, CorrelationIdentifierOptions>(builder, setup);
        }

        /// <summary>
        /// Adds a request identifier HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="RequestIdentifierOptions"/> middleware which may be configured.</param>
        /// <returns>An <see cref="IApplicationBuilder"/> that can be used to further configure the request pipeline.</returns>
        /// <remarks>Default HTTP header name is <c>X-Request-ID</c>.</remarks>
        public static IApplicationBuilder UseRequestIdentifier(this IApplicationBuilder builder, Action<RequestIdentifierOptions> setup = null)
        {
            return MiddlewareBuilderFactory.UseConfigurableMiddleware<RequestIdentifierMiddleware, RequestIdentifierOptions>(builder, setup);
        }

        /// <summary>
        /// Adds a HTTP User-Agent header parser to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="UserAgentSentinelOptions"/> middleware which may be configured.</param>
        /// <returns>An <see cref="IApplicationBuilder"/> that can be used to further configure the request pipeline.</returns>
        public static IApplicationBuilder UseUserAgentSentinel(this IApplicationBuilder builder, Action<UserAgentSentinelOptions> setup = null)
        {
            return MiddlewareBuilderFactory.UseConfigurableMiddleware<UserAgentSentinelMiddleware, UserAgentSentinelOptions>(builder, setup);
        }

        /// <summary>
        /// Adds an API key header parser to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="ApiKeySentinelOptions"/> middleware which may be configured.</param>
        /// <returns>An <see cref="IApplicationBuilder"/> that can be used to further configure the request pipeline.</returns>
        public static IApplicationBuilder UseApiKeySentinel(this IApplicationBuilder builder, Action<ApiKeySentinelOptions> setup = null)
        {
            return MiddlewareBuilderFactory.UseConfigurableMiddleware<ApiKeySentinelMiddleware, ApiKeySentinelOptions>(builder, setup);
        }

        /// <summary>
        /// Adds an HTTP Cache-Control and HTTP Expires header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="CacheableOptions"/> middleware which may be configured.</param>
        /// <returns>An <see cref="IApplicationBuilder"/> that can be used to further configure the request pipeline.</returns>
        /// <returns>An <see cref="IApplicationBuilder"/> that can be used to further configure the request pipeline.</returns>
        public static IApplicationBuilder UseCacheControl(this IApplicationBuilder builder, Action<CacheableOptions> setup = null)
        {
            return MiddlewareBuilderFactory.UseConfigurableMiddleware<CacheableMiddleware, CacheableOptions>(builder, setup);
        }

        /// <summary>
        /// Adds an HTTP Vary: Accept header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application's request pipeline.</param>
        /// <returns>An <see cref="IApplicationBuilder"/> that can be used to further configure the request pipeline.</returns>
        public static IApplicationBuilder UseVaryAccept(this IApplicationBuilder builder)
        {
            return MiddlewareBuilderFactory.UseMiddleware<VaryAcceptMiddleware>(builder);
        }
    }
}