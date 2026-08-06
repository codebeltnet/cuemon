using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Diagnostics;
/// <summary>
/// Extension methods for the <see cref="IApplicationBuilder"/> interface.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds a Server-Timing HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
    /// </summary>
    /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IApplicationBuilder UseServerTiming(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ServerTimingMiddleware>();
    }

    /// <summary>
    /// Adds a middleware to the pipeline that will catch exceptions, log them, and re-execute the request in an alternate pipeline. The request will not be re-executed if the response has already started.
    /// </summary>
    /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>Extends the existing <see cref="ExceptionHandlerMiddleware"/> to include features similar to those provided by Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter, except:<br/>
    /// 1. Unable to interact with controller applied attributes (outside scope; part of MVC context)<br/>
    /// 2. Unable to mark an exception as handled (outside scope; part of MVC context)
    /// </remarks>
    public static IApplicationBuilder UseFaultDescriptorExceptionHandler(this IApplicationBuilder builder)
    {
        var handlerOptions = new ExceptionHandlerOptions()
        {
            ExceptionHandler = async context =>
            {
                var ehf = context.Features.Get<IExceptionHandlerFeature>();
                if (ehf != null)
                {
                    var options = context.RequestServices.GetRequiredService<IOptions<FaultDescriptorOptions>>().Value;
                    var nonMvcResponseHandlers = context.RequestServices.GetExceptionResponseFormatters().SelectExceptionDescriptorHandlers();

                    if (Decorator.Enclose(options).TryResolveHttpExceptionDescriptor(ehf.Error, context, null, out var descriptor))
                    {
                        context.Response.StatusCode = descriptor.StatusCode;
                    }

                    var handlers = new List<HttpExceptionDescriptorResponseHandler>();
                    handlers = handlers.Concat(nonMvcResponseHandlers).ToList();

                    var accepts = context.Request.AcceptMimeTypesOrderedByQuality();

                    foreach (var accept in accepts)
                    {
                        var handler = handlers.FirstOrDefault(rh => rh.ContentType.MediaType != null && rh.ContentType.MediaType.Equals(accept, StringComparison.OrdinalIgnoreCase));
                        if (handler != null)
                        {
                            await WriteResponseAsync(context, handler, descriptor, options.CancellationToken).ConfigureAwait(false);
                            return;
                        }
                    }

                    var fallback = HttpExceptionDescriptorResponseHandler.CreateDefaultFallbackHandler(context.RequestServices.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value.SensitivityDetails);
                    await WriteResponseAsync(context, fallback, descriptor, options.CancellationToken).ConfigureAwait(false); // fallback in case no match from Accept header
                }
            }
        };
        return builder.UseExceptionHandler(handlerOptions);
    }

    private static Task WriteResponseAsync(HttpContext context, HttpExceptionDescriptorResponseHandler handler, HttpExceptionDescriptor exceptionDescriptor, CancellationToken ct)
    {
        return Decorator.Enclose(context).WriteExceptionDescriptorResponseAsync(handler, exceptionDescriptor, ct);
    }
}
