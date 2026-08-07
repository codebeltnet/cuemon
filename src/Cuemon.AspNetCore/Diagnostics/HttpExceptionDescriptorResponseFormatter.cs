using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.Configuration;
using Cuemon.Net.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Provides a generic way to support content negotiation for exceptions in the application.
/// </summary>
/// <typeparam name="TOptions">The type of the options to configure.</typeparam>
/// <seealso cref="Configurable{TOptions}" />
/// <seealso cref="IHttpExceptionDescriptorResponseFormatter" />
/// <seealso cref="ExceptionHandlerMiddleware"/>
public class HttpExceptionDescriptorResponseFormatter<TOptions> : Configurable<TOptions>, IHttpExceptionDescriptorResponseFormatter
    where TOptions : class, IContentNegotiation, IParameterObject, new()
{
    private ICollection<HttpExceptionDescriptorResponseHandler> _exceptionDescriptorHandlers = new List<HttpExceptionDescriptorResponseHandler>();

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpExceptionDescriptorResponseFormatter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> could not be configured to a valid state.
    /// </exception>
    public HttpExceptionDescriptorResponseFormatter(Action<TOptions> setup) : this(Validator.CheckParameter(() =>
    {
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        return options;
    }))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpExceptionDescriptorResponseFormatter{TOptions}"/> class.
    /// </summary>
    /// <param name="options">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="options"/> are not in a valid state.
    /// </exception>
    public HttpExceptionDescriptorResponseFormatter(IOptions<TOptions> options) : this(options?.Value)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpExceptionDescriptorResponseFormatter{TOptions}"/> class.
    /// </summary>
    /// <param name="options">The configured options of this instance.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="options"/> are not in a valid state.
    /// </exception>
    public HttpExceptionDescriptorResponseFormatter(TOptions options) : base(options)
    {
    }

    /// <summary>
    /// Gets the collection of populated <see cref="HttpExceptionDescriptorResponseHandler"/> instances.
    /// </summary>
    /// <value>The collection of populated <see cref="HttpExceptionDescriptorResponseHandler"/> instances.</value>
    public ICollection<HttpExceptionDescriptorResponseHandler> ExceptionDescriptorHandlers => _exceptionDescriptorHandlers;

    /// <summary>
    /// Adjusts the <see cref="Options"/> to your liking.
    /// </summary>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    /// <returns>A reference to this instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> could not be configured to a valid state.
    /// </exception>
    public HttpExceptionDescriptorResponseFormatter<TOptions> Adjust(Action<TOptions> setup)
    {
        Validator.ThrowIfInvalidConfigurator(setup, out _);
        setup.Invoke(Options);
        return this;
    }

    /// <summary>
    /// Populates the underlying <see cref="IContentNegotiation.SupportedMediaTypes"/> to the <see cref="ExceptionDescriptorHandlers"/> using the specified <paramref name="contentFactory"/>.
    /// </summary>
    /// <param name="contentFactory">The function delegate that will create an instance of <see cref="HttpContent"/> with input from the specified <see cref="HttpExceptionDescriptor"/> and <see cref="MediaTypeHeaderValue"/>.</param>
    /// <param name="exceptionDescriptorHandlers">The optional collection of <see cref="HttpExceptionDescriptorResponseHandler"/> instances to populate to.</param>
    /// <returns>A reference to this instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="contentFactory"/> cannot be null.
    /// </exception>
    public HttpExceptionDescriptorResponseFormatter<TOptions> Populate(Func<HttpExceptionDescriptor, MediaTypeHeaderValue, HttpContent> contentFactory, ICollection<HttpExceptionDescriptorResponseHandler> exceptionDescriptorHandlers = null)
    {
        Validator.ThrowIfNull(contentFactory);
        if (exceptionDescriptorHandlers != null) { _exceptionDescriptorHandlers = exceptionDescriptorHandlers; }
        foreach (var mediaType in Options.SupportedMediaTypes)
        {
            Decorator.Enclose(_exceptionDescriptorHandlers).AddResponseHandler(o =>
            {
                o.ContentType = mediaType;
                o.ContentFactory = descriptor => contentFactory(descriptor, mediaType);
                o.StatusCodeFactory = ed => (HttpStatusCode)ed.StatusCode;
            });
        }
        return this;
    }
}
