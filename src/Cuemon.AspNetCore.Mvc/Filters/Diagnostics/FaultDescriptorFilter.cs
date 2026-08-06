using System;
using System.Net;
using System.Reflection;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
/// <summary>
/// A filter that, after an action has faulted, provides developer friendly information about an <see cref="Exception"/> along with a correct <see cref="HttpStatusCode"/>.
/// </summary>
/// <seealso cref="IExceptionFilter"/>
/// <seealso cref="ExceptionDescriptor" />
/// <seealso cref="MvcFaultDescriptorOptions"/>
/// <seealso cref="RequestIdentifierMiddleware"/>
/// <seealso cref="CorrelationIdentifierMiddleware"/>
public class FaultDescriptorFilter : Configurable<MvcFaultDescriptorOptions>, IExceptionFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FaultDescriptorFilter"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="MvcFaultDescriptorOptions"/> which need to be configured.</param>
    public FaultDescriptorFilter(IOptions<MvcFaultDescriptorOptions> setup) : base(Validator.CheckParameter(() =>
    {
        Validator.ThrowIfInvalidOptions(setup.Value, paramName: nameof(setup));
        return setup.Value;
    }))
    {
    }

    /// <summary>
    /// Called after an action has thrown an <see cref="Exception" />.
    /// </summary>
    /// <param name="context">The <see cref="ExceptionContext" />.</param>
    public virtual void OnException(ExceptionContext context)
    {
        if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor
            && Decorator.Enclose(Options).TryResolveHttpExceptionDescriptor(context.Exception, context.HttpContext, ed => ed.PostInitializeWith(actionDescriptor.MethodInfo.GetCustomAttributes<ExceptionDescriptorAttribute>()), out var descriptor))
        {
            context.HttpContext.Response.StatusCode = descriptor.StatusCode;

            if (Options.MarkExceptionHandled) { context.ExceptionHandled = true; }

            switch (Options.FaultDescriptor)
            {
                case PreferredFaultDescriptor.FaultDetails:
                    context.Result = new ExceptionDescriptorResult(descriptor);
                    break;
                default:
                    context.Result = new ExceptionDescriptorResult(Decorator.Enclose(descriptor).ToProblemDetails(Options.SensitivityDetails));
                    break;
            }
        }
    }
}
