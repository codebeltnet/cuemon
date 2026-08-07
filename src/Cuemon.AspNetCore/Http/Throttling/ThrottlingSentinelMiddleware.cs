using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Http.Throttling;
/// <summary>
/// Provides an API throttling middleware implementation for ASP.NET Core.
/// </summary>
public class ThrottlingSentinelMiddleware : ConfigurableMiddleware<IThrottlingCache, ThrottlingSentinelOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThrottlingSentinelMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="ThrottlingSentinelOptions" /> which need to be configured.</param>
    public ThrottlingSentinelMiddleware(RequestDelegate next, IOptions<ThrottlingSentinelOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThrottlingSentinelMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="ThrottlingSentinelOptions" /> which need to be configured.</param>
    public ThrottlingSentinelMiddleware(RequestDelegate next, Action<ThrottlingSentinelOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="ThrottlingSentinelMiddleware" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di">The dependency injected <see cref="IThrottlingCache"/> of <see cref="InvokeAsync"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public override async Task InvokeAsync(HttpContext context, IThrottlingCache di)
    {
        await Decorator.Enclose(context).InvokeThrottlerSentinelAsync(di, Options).ConfigureAwait(false);
        await Next(context).ConfigureAwait(false);
    }
}
