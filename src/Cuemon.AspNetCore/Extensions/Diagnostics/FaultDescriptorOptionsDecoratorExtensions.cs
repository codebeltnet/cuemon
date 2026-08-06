using System;
using System.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Extension methods for <see cref="FaultDescriptorOptions"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
public static class FaultDescriptorOptionsDecoratorExtensions
{
    /// <summary>
    /// Tries to resolve an <see cref="HttpExceptionDescriptor"/> from the specified <paramref name="failure"/> and <paramref name="context"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="FaultDescriptorOptions"/>.</typeparam>
    /// <param name="decorator">The decorator that encapsulates the <see cref="FaultDescriptorOptions"/>.</param>
    /// <param name="failure">The <see cref="Exception"/> that represents the failure.</param>
    /// <param name="context">The <see cref="HttpContext"/> in which the failure occurred.</param>
    /// <param name="onBeforeExceptionFactory">A delegate to invoke just before the <see cref="FaultDescriptorOptions.ExceptionCallback"/> is called.</param>
    /// <param name="descriptor">When this method returns, contains the resolved <see cref="HttpExceptionDescriptor"/>, if the resolution succeeded, or <c>null</c> if the resolution failed.</param>
    /// <returns><c>true</c> if the <see cref="HttpExceptionDescriptor"/> was resolved successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="failure"/> cannot be null -or-
    /// <paramref name="context"/> cannot be null.
    /// </exception>
    public static bool TryResolveHttpExceptionDescriptor<T>(this IDecorator<T> decorator, Exception failure, HttpContext context, Action<HttpExceptionDescriptor> onBeforeExceptionFactory, out HttpExceptionDescriptor descriptor) where T : FaultDescriptorOptions
    {
        Validator.ThrowIfNull(decorator, out var options);
        Validator.ThrowIfNull(failure);
        Validator.ThrowIfNull(context);

        descriptor = options.ExceptionDescriptorResolver?.Invoke(options.UseBaseException ? failure.GetBaseException() : failure);
        if (descriptor == null) { return false; }

        if (options.HasRootHelpLink && descriptor.HelpLink == null) { descriptor.HelpLink = options.RootHelpLink; }

        if (context.Items.TryGetValue(RequestIdentifierMiddleware.HttpContextItemsKey, out var requestId) && requestId != null) { descriptor.RequestId = requestId.ToString(); }
        if (context.Items.TryGetValue(CorrelationIdentifierMiddleware.HttpContextItemsKey, out var correlationId) && correlationId != null) { descriptor.CorrelationId = correlationId.ToString(); }
        descriptor.TraceId = Activity.Current?.Id ?? context.TraceIdentifier;

        if (Uri.TryCreate(context.Request.GetDisplayUrl(), UriKind.Absolute, out var instance))
        {
            descriptor.Instance = instance;
        }

        if (options.RequestEvidenceProvider != null && options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence)) { descriptor.AddEvidence("Request", context.Request, options.RequestEvidenceProvider); }
        if (failure is HttpStatusCodeException httpFault)
        {
            Decorator.Enclose(context.Response.Headers).AddRange(httpFault.Headers);
        }

        onBeforeExceptionFactory?.Invoke(descriptor);
        options.ExceptionCallback?.Invoke(context, failure, descriptor);

        return true;
    }
}
