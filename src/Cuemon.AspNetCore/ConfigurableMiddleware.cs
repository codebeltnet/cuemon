using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Infrastructure;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore;

/// <summary>
/// Provides an base-class for configurable middleware implementation in ASP.NET Core that supports the options pattern.
/// </summary>
/// <typeparam name="TOptions">The type of the options to setup.</typeparam>
/// <seealso cref="Middleware" />
public abstract class ConfigurableMiddleware<TOptions> : ConfigurableMiddlewareCore<TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, IOptions<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, Action<TOptions> setup) : base(next, setup)
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
/// Provides an base-class for configurable middleware implementation in ASP.NET Core that supports the options pattern with a dependency injected parameter.
/// </summary>
/// <typeparam name="T">The type of the dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="TOptions">The type of the options to setup.</typeparam>
/// <seealso cref="Middleware{T}" />
public abstract class ConfigurableMiddleware<T, TOptions> : ConfigurableMiddlewareCore<TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, IOptions<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, Action<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="ConfigurableMiddleware{T,TOptions}" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di">The dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public abstract Task InvokeAsync(HttpContext context, T di);
}

/// <summary>
/// Provides an base-class for configurable middleware implementation in ASP.NET Core that supports the options pattern with two dependency injected parameters.
/// </summary>
/// <typeparam name="T1">The type of the first dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T2">The type of the second dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="TOptions">The type of the options to setup.</typeparam>
/// <seealso cref="Middleware{T1,T2}" />
public abstract class ConfigurableMiddleware<T1, T2, TOptions> : ConfigurableMiddlewareCore<TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, IOptions<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, Action<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="ConfigurableMiddleware{T1,T2,TOptions}" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di1">The first dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di2">The second dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public abstract Task InvokeAsync(HttpContext context, T1 di1, T2 di2);
}

/// <summary>
/// Provides an base-class for configurable middleware implementation in ASP.NET Core that supports the options pattern with three dependency injected parameters.
/// </summary>
/// <typeparam name="T1">The type of the first dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T2">The type of the second dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T3">The type of the third dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="TOptions">The type of the options to setup.</typeparam>
/// <seealso cref="Middleware{T1,T2,T3}" />
public abstract class ConfigurableMiddleware<T1, T2, T3, TOptions> : ConfigurableMiddlewareCore<TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, IOptions<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, Action<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="ConfigurableMiddleware{T1,T2,T3,TOptions}" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di1">The first dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di2">The second dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <param name="di3">The third dependency injected parameter of <see cref="InvokeAsync"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public abstract Task InvokeAsync(HttpContext context, T1 di1, T2 di2, T3 di3);
}

/// <summary>
/// Provides an base-class for configurable middleware implementation in ASP.NET Core that supports the options pattern with four dependency injected parameters.
/// </summary>
/// <typeparam name="T1">The type of the first dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T2">The type of the second dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T3">The type of the third dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="TOptions">The type of the options to setup.</typeparam>
/// <seealso cref="Middleware{T1,T2,T3,T4}" />
public abstract class ConfigurableMiddleware<T1, T2, T3, T4, TOptions> : ConfigurableMiddlewareCore<TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, IOptions<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, Action<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="ConfigurableMiddleware{T1,T2,T3,T4,TOptions}" />.
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
/// Provides an base-class for configurable middleware implementation in ASP.NET Core that supports the options pattern with five dependency injected parameters.
/// </summary>
/// <typeparam name="T1">The type of the first dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T2">The type of the second dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T3">The type of the third dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth dependency injected parameter of <see cref="InvokeAsync"/>.</typeparam>
/// <typeparam name="TOptions">The type of the options to setup.</typeparam>
/// <seealso cref="Middleware{T1,T2,T3,T4,T5}" />
public abstract class ConfigurableMiddleware<T1, T2, T3, T4, T5, TOptions> : ConfigurableMiddlewareCore<TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, IOptions<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableMiddleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    protected ConfigurableMiddleware(RequestDelegate next, Action<TOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="ConfigurableMiddleware{T1,T2,T3,T4,T5,TOptions}" />.
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
