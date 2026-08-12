using System.Threading.Tasks;
using Cuemon.AspNetCore.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore;
/// <summary>
/// Provides a base-class for middleware implementation in ASP.NET Core.
/// </summary>
public abstract class Middleware : MiddlewareCore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Middleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    protected Middleware(RequestDelegate next) : base(next)
    {
    }

    /// <summary>
    /// Executes the <see cref="Middleware"/>.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public abstract Task InvokeAsync(HttpContext context);
}

/// <summary>
/// Provides a base-class for middleware implementation in ASP.NET Core with one dependency injected parameters.
/// </summary>
/// <typeparam name="T">The type of the dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <seealso cref="ConfigurableMiddleware{T,TOptions}" />
public abstract class Middleware<T> : MiddlewareCore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Middleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    protected Middleware(RequestDelegate next) : base(next)
    {
    }

    /// <summary>
    /// Executes the <see cref="Middleware{T}" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di">The dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public abstract Task InvokeAsync(HttpContext context, T di);
}

/// <summary>
/// Provides a base-class for middleware implementation in ASP.NET Core with two dependency injected parameters.
/// </summary>
/// <typeparam name="T1">The type of the first dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T2">The type of the second dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <seealso cref="ConfigurableMiddleware{T1,T2,TOptions}" />
public abstract class Middleware<T1, T2> : MiddlewareCore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Middleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    protected Middleware(RequestDelegate next) : base(next)
    {
    }

    /// <summary>
    /// Executes the <see cref="Middleware{T1,T2}" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di1">The first dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di2">The second dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public abstract Task InvokeAsync(HttpContext context, T1 di1, T2 di2);
}

/// <summary>
/// Provides a base-class for middleware implementation in ASP.NET Core with three dependency injected parameters.
/// </summary>
/// <typeparam name="T1">The type of the first dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T2">The type of the second dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T3">The type of the third dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <seealso cref="ConfigurableMiddleware{T1,T2,T3,TOptions}" />
public abstract class Middleware<T1, T2, T3> : MiddlewareCore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Middleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    protected Middleware(RequestDelegate next) : base(next)
    {
    }

    /// <summary>
    /// Executes the <see cref="Middleware{T1,T2,T3}" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di1">The first dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di2">The second dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di3">The third dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public abstract Task InvokeAsync(HttpContext context, T1 di1, T2 di2, T3 di3);
}

/// <summary>
/// Provides a base-class for middleware implementation in ASP.NET Core with four dependency injected parameters.
/// </summary>
/// <typeparam name="T1">The type of the first dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T2">The type of the second dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T3">The type of the third dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <seealso cref="ConfigurableMiddleware{T1,T2,T3,T4,TOptions}" />
public abstract class Middleware<T1, T2, T3, T4> : MiddlewareCore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Middleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    protected Middleware(RequestDelegate next) : base(next)
    {
    }

    /// <summary>
    /// Executes the <see cref="Middleware{T1,T2,T3,T4}" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di1">The first dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di2">The second dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di3">The third dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di4">The fourth dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public abstract Task InvokeAsync(HttpContext context, T1 di1, T2 di2, T3 di3, T4 di4);
}

/// <summary>
/// Provides a base-class for middleware implementation in ASP.NET Core with five dependency injected parameters.
/// </summary>
/// <typeparam name="T1">The type of the first dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T2">The type of the second dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T3">The type of the third dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <seealso cref="ConfigurableMiddleware{T1,T2,T3,T4,T5,TOptions}" />
public abstract class Middleware<T1, T2, T3, T4, T5> : MiddlewareCore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Middleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    protected Middleware(RequestDelegate next) : base(next)
    {
    }

    /// <summary>
    /// Executes the <see cref="Middleware{T1,T2,T3,T4,T5}" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di1">The first dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di2">The second dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di3">The third dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di4">The fourth dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di5">The fifth dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public abstract Task InvokeAsync(HttpContext context, T1 di1, T2 di2, T3 di3, T4 di4, T5 di5);
}
