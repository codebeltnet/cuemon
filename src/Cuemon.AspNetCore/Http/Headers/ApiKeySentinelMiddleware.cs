using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// Provides an API key sentinel middleware implementation for ASP.NET Core.
/// </summary>
public class ApiKeySentinelMiddleware : ConfigurableMiddleware<ApiKeySentinelOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeySentinelMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="UserAgentSentinelOptions" /> which need to be configured.</param>
    public ApiKeySentinelMiddleware(RequestDelegate next, IOptions<ApiKeySentinelOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeySentinelMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="UserAgentSentinelOptions" /> which need to be configured.</param>
    public ApiKeySentinelMiddleware(RequestDelegate next, Action<ApiKeySentinelOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="UserAgentSentinelMiddleware" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public override async Task InvokeAsync(HttpContext context)
    {
        await Decorator.Enclose(context).InvokeApiKeySentinelAsync(Options).ConfigureAwait(false);
        await Next(context).ConfigureAwait(false);
    }
}
