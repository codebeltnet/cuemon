using System;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// Provides a Correlation ID middleware implementation for ASP.NET Core.
/// </summary>
public class CorrelationIdentifierMiddleware : ConfigurableMiddleware<CorrelationIdentifierOptions>
{
    /// <summary>
    /// The key from where the Correlation ID is stored throughout the request scope.
    /// </summary>
    public const string HttpContextItemsKey = "Cuemon.AspNetCore.Http.Headers.CorrelationIdentifierMiddleware";

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationIdentifierMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="CorrelationIdentifierOptions" /> which need to be configured.</param>
    public CorrelationIdentifierMiddleware(RequestDelegate next, IOptions<CorrelationIdentifierOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationIdentifierMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="CorrelationIdentifierOptions"/> which need to be configured.</param>
    public CorrelationIdentifierMiddleware(RequestDelegate next, Action<CorrelationIdentifierOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="CorrelationIdentifierMiddleware" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public override Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(Options.HeaderName, out var correlationId)) { correlationId = Options.Token.CorrelationId; }
        Decorator.Enclose(context.Items).TryAdd(HttpContextItemsKey, correlationId);
        context.Response.OnStarting(() =>
        {
            Decorator.Enclose(context.Response.Headers).AddOrUpdate(Options.HeaderName, correlationId);
            return Task.CompletedTask;
        });
        return Next(context);
    }
}
