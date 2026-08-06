using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Infrastructure;
/// <summary>
/// Provides a base-class for middleware implementation in ASP.NET Core.
/// This API supports the product infrastructure and is not intended to be used directly from your code.
/// </summary>
public abstract class MiddlewareCore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Middleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    internal MiddlewareCore(RequestDelegate next)
    {
        Validator.ThrowIfNull(next);
        Next = next;
    }

    /// <summary>
    /// Gets the delegate of the request pipeline to invoke.
    /// </summary>
    /// <value>The delegate of the request pipeline to invoke.</value>
    protected RequestDelegate Next { get; }
}
