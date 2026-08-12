using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics;
/// <summary>
/// Extension methods for the <see cref="HttpFaultResolver"/> class.
/// </summary>
public static class HttpFaultResolverExtensions
{
    /// <summary>
    /// Adds a new <see cref="HttpExceptionDescriptor"/> to the collection of <paramref name="descriptors"/> from the parameters provided.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="HttpStatusCodeException"/> to associate with a <see cref="HttpFaultResolver"/>.</typeparam>
    /// <param name="descriptors">The collection to extend.</param>
    /// <param name="message">The message that explains the reason for the failure.</param>
    /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
    /// <param name="exceptionValidator">The function delegate that evaluates an <see cref="Exception"/>.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptors"/> is null.
    /// </exception>
    /// <remarks>
    /// The following table shows the initial property values for the added instance of <see cref="HttpExceptionDescriptor"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Parameter</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><paramref name="message"/></term>
    ///         <description><c>message ?? failure.Message</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public static IList<HttpFaultResolver> AddHttpFaultResolver<T>(this IList<HttpFaultResolver> descriptors, string message = null, Uri helpLink = null, Func<Exception, bool> exceptionValidator = null) where T : HttpStatusCodeException
    {
        Validator.ThrowIfNull(descriptors);
        return Decorator.Enclose(descriptors).AddHttpFaultResolver<T>(message, helpLink, exceptionValidator).Inner;
    }

    /// <summary>
    /// Adds a new <see cref="HttpExceptionDescriptor"/> to the collection of <paramref name="descriptors"/> from the parameters provided.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Exception"/> to associate with a <see cref="HttpFaultResolver"/>.</typeparam>
    /// <param name="descriptors">The collection to extend.</param>
    /// <param name="statusCode">The status code of the HTTP request.</param>
    /// <param name="code">The error code that uniquely identifies the type of failure.</param>
    /// <param name="message">The message that explains the reason for the failure.</param>
    /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
    /// <param name="exceptionValidator">The function delegate that evaluates an <see cref="Exception"/>.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptors"/> is null.
    /// </exception>
    /// <remarks>
    /// The following table shows the initial property values for the added instance of <see cref="HttpExceptionDescriptor"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Parameter</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><paramref name="code"/></term>
    ///         <description><c>code ?? ReasonPhrases.GetReasonPhrase(statusCode)</c></description>
    ///     </item>
    ///     <item>
    ///         <term><paramref name="message"/></term>
    ///         <description><c>message ?? failure.Message</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public static IList<HttpFaultResolver> AddHttpFaultResolver<T>(this IList<HttpFaultResolver> descriptors, int statusCode, string code = null, string message = null, Uri helpLink = null, Func<Exception, bool> exceptionValidator = null) where T : Exception
    {
        Validator.ThrowIfNull(descriptors);
        return Decorator.Enclose(descriptors).AddHttpFaultResolver<T>(statusCode, code, message, helpLink, exceptionValidator).Inner;
    }

    /// <summary>
    /// Adds the specified function delegate <paramref name="exceptionDescriptorResolver"/> and function delegate <paramref name="exceptionValidator"/> to the collection of <paramref name="descriptors"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Exception"/> to associate with a <see cref="HttpFaultResolver"/>.</typeparam>
    /// <param name="descriptors">The collection to extend.</param>
    /// <param name="exceptionDescriptorResolver">The function delegate that associates an <see cref="Exception"/> of type <typeparamref name="T"/> with an <see cref="HttpExceptionDescriptor"/>.</param>
    /// <param name="exceptionValidator">The function delegate that evaluates an <see cref="Exception"/>.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptors"/> is null.
    /// </exception>
    public static IList<HttpFaultResolver> AddHttpFaultResolver<T>(this IList<HttpFaultResolver> descriptors, Func<T, HttpExceptionDescriptor> exceptionDescriptorResolver, Func<Exception, bool> exceptionValidator) where T : Exception
    {
        Validator.ThrowIfNull(descriptors);
        return Decorator.Enclose(descriptors).AddHttpFaultResolver(exceptionDescriptorResolver, exceptionValidator).Inner;
    }
}
