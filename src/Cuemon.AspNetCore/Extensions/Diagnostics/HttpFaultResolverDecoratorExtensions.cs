using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Http;

namespace Cuemon.AspNetCore.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="HttpFaultResolver"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class HttpFaultResolverDecoratorExtensions
    {
        /// <summary>
        /// Adds a new <see cref="HttpExceptionDescriptor"/> to the enclosed <see cref="IList{FaultResolver}"/> of the <paramref name="decorator"/> from the parameters provided.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="HttpStatusCodeException"/> to associate with a <see cref="HttpFaultResolver"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="message">The message that explains the reason for the failure.</param>
        /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
        /// <param name="exceptionValidator">The function delegate that evaluates an <see cref="Exception"/>.</param>
        /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
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
        public static IDecorator<IList<HttpFaultResolver>> AddHttpFaultResolver<T>(this IDecorator<IList<HttpFaultResolver>> decorator, string message = null, Uri helpLink = null, Func<Exception, bool> exceptionValidator = null) where T : HttpStatusCodeException
        {
            return AddHttpFaultResolver<T>(decorator, ex => new HttpExceptionDescriptor(ex, ex.StatusCode, ex.ReasonPhrase, message, helpLink), exceptionValidator);
        }

        /// <summary>
        /// Adds a new <see cref="HttpExceptionDescriptor"/> to the enclosed <see cref="IList{FaultResolver}"/> of the <paramref name="decorator"/> from the parameters provided.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Exception"/> to associate with a <see cref="HttpFaultResolver"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="statusCode">The status code of the HTTP request.</param>
        /// <param name="code">The error code that uniquely identifies the type of failure.</param>
        /// <param name="message">The message that explains the reason for the failure.</param>
        /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
        /// <param name="exceptionValidator">The function delegate that evaluates an <see cref="Exception"/>.</param>
        /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
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
        public static IDecorator<IList<HttpFaultResolver>> AddHttpFaultResolver<T>(this IDecorator<IList<HttpFaultResolver>> decorator, int statusCode, string code = null, string message = null, Uri helpLink = null, Func<Exception, bool> exceptionValidator = null) where T : Exception
        {
            return AddHttpFaultResolver<T>(decorator, ex => new HttpExceptionDescriptor(ex, statusCode, code, message, helpLink), exceptionValidator);
        }

        /// <summary>
        /// Adds the specified function delegate <paramref name="exceptionDescriptorResolver"/> and function delegate <paramref name="exceptionValidator"/> to the enclosed <see cref="IList{FaultResolver}"/> of the <paramref name="decorator"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Exception"/> to associate with a <see cref="HttpFaultResolver"/>.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="exceptionDescriptorResolver">The function delegate that associates an <see cref="Exception"/> of type <typeparamref name="T"/> with an <see cref="HttpExceptionDescriptor"/>.</param>
        /// <param name="exceptionValidator">The function delegate that evaluates an <see cref="Exception"/>.</param>
        /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static IDecorator<IList<HttpFaultResolver>> AddHttpFaultResolver<T>(this IDecorator<IList<HttpFaultResolver>> decorator, Func<T, HttpExceptionDescriptor> exceptionDescriptorResolver, Func<Exception, bool> exceptionValidator) where T : Exception
        {
            Validator.ThrowIfNull(decorator);
            exceptionValidator ??= ex => ex is T;
            decorator.Inner.Add(new HttpFaultResolver(exceptionValidator, ex => exceptionDescriptorResolver((T)ex)));
            return decorator;
        }
    }
}