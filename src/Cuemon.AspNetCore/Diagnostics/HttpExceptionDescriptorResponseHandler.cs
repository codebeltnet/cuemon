using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.Diagnostics;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Provides a way to support content negotiation for <see cref="HttpExceptionDescriptor" />.
/// </summary>
public class HttpExceptionDescriptorResponseHandler
{
    private readonly Func<HttpExceptionDescriptor, HttpResponseMessage> _responseMessageFactory;

    /// <summary>
    /// Creates a scaled down <see cref="HttpExceptionDescriptorResponseHandler"/> suitable as fallback handler for when content negotiation fails.
    /// </summary>
    /// <param name="sensitivityDetails">The bitwise combination of the enumeration values that specify which sensitive details to include in the serialized result.</param>
    /// <returns>A new instance of a scaled down <see cref="HttpExceptionDescriptorResponseHandler"/>.</returns>
    /// <remarks>This scaled down implementation of a fallback handler requires <see cref="FaultSensitivityDetails.All"/> to provide developer friendly insights about an <see cref="Exception"/>. Do not use in Production environment.</remarks>
    public static HttpExceptionDescriptorResponseHandler CreateDefaultFallbackHandler(FaultSensitivityDetails sensitivityDetails)
    {
        var contentType = new MediaTypeHeaderValue("text/plain");
        return new HttpExceptionDescriptorResponseHandler(contentType, descriptor => new HttpResponseMessage((HttpStatusCode)descriptor.StatusCode)
        {
            Content = new StreamContent(sensitivityDetails == FaultSensitivityDetails.All ? Decorator.Enclose(descriptor.ToString()).ToStream() : Decorator.Enclose(descriptor.Message).ToStream()) // for security reasons (and to reduce complexity) only use Exception.ToString() for FaultSensitivityDetails.All; all other cases use Message
            {
                Headers = { ContentType = contentType }
            }
        });
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpExceptionDescriptorResponseHandler"/> class.
    /// </summary>
    /// <param name="contentType">The media type that this handler supports.</param>
    /// <param name="responseMessageFactory">The function delegate that produces the <see cref="HttpResponseMessage"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="contentType"/> cannot be null -or-
    /// <paramref name="responseMessageFactory"/> cannot be null.
    /// </exception>
    public HttpExceptionDescriptorResponseHandler(MediaTypeHeaderValue contentType, Func<HttpExceptionDescriptor, HttpResponseMessage> responseMessageFactory)
    {
        Validator.ThrowIfNull(contentType);
        Validator.ThrowIfNull(responseMessageFactory);

        ContentType = contentType;

        _responseMessageFactory = responseMessageFactory;
    }

    /// <summary>
    /// Gets the media type that this handler supports.
    /// </summary>
    /// <value>The media type that this handler supports.</value>
    public MediaTypeHeaderValue ContentType { get; }

    /// <summary>
    /// Converts this instance into an <see cref="HttpResponseMessage"/> from the by constructor provided factory.
    /// </summary>
    /// <param name="descriptor">The exception descriptor tailored for HTTP requests.</param>
    /// <returns>An <see cref="HttpResponseMessage"/> from the by constructor provided factory.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> cannot be null.
    /// </exception>
    public HttpResponseMessage ToHttpResponseMessage(HttpExceptionDescriptor descriptor)
    {
        return _responseMessageFactory(descriptor);
    }
}
