using System;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// Provides a Request ID middleware implementation for ASP.NET Core.
/// </summary>
public class RequestIdentifierMiddleware : ConfigurableMiddleware<RequestIdentifierOptions>
{
    /// <summary>
    /// The key from where the Request ID is stored throughout the request scope.
    /// </summary>
    public const string HttpContextItemsKey = "Cuemon.AspNetCore.Http.Headers.RequestIdentifierMiddleware";

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestIdentifierMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="RequestIdentifierOptions" /> which need to be configured.</param>
    public RequestIdentifierMiddleware(RequestDelegate next, IOptions<RequestIdentifierOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestIdentifierMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="RequestIdentifierOptions"/> which need to be configured.</param>
    public RequestIdentifierMiddleware(RequestDelegate next, Action<RequestIdentifierOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="RequestIdentifierMiddleware" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public override Task InvokeAsync(HttpContext context)
    {
        var requestId = Options.Token.RequestId;
        Decorator.Enclose(context.Items).TryAdd(HttpContextItemsKey, requestId);
        context.Response.OnStarting(() =>
        {
            Decorator.Enclose(context.Response.Headers).AddOrUpdate(Options.HeaderName, requestId);
            return Task.CompletedTask;
        });
        return Next(context);
    }
}
