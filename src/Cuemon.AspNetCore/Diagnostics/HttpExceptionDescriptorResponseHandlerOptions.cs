using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.Configuration;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Specifies options that is related to <see cref="HttpExceptionDescriptorResponseHandler" /> operations.
/// </summary>
/// <seealso cref="IValidatableParameterObject"/>
public class HttpExceptionDescriptorResponseHandlerOptions : IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpExceptionDescriptorResponseHandlerOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="HttpExceptionDescriptorResponseHandlerOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="ContentFactory"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ContentType"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="StatusCodeFactory"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public HttpExceptionDescriptorResponseHandlerOptions()
    {
    }

    /// <summary>
    /// Gets or sets the function delegate that will create an instance of <see cref="HttpContent"/> with input from the specified <see cref="HttpExceptionDescriptor"/>.
    /// </summary>
    /// <value>The function delegate that will create an instance of <see cref="HttpContent"/> with input from the specified <see cref="HttpExceptionDescriptor"/>.</value>
    /// <remarks>This property is required and cannot be null.</remarks>
    public Func<HttpExceptionDescriptor, HttpContent> ContentFactory { get; set; }

    /// <summary>
    /// Gets or sets the supported media-type of the response.
    /// </summary>
    /// <value>The supported media-type of the response.</value>
    /// <remarks>This property is required and cannot be null.</remarks>
    public MediaTypeHeaderValue ContentType { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that will return a <see cref="HttpStatusCode"/> value with input from the specified <see cref="HttpExceptionDescriptor"/>.
    /// </summary>
    /// <value>The function delegate that will return a <see cref="HttpStatusCode"/> value with input from the specified <see cref="HttpExceptionDescriptor"/>.</value>
    /// <remarks>This property is required and cannot be null.</remarks>
    public Func<HttpExceptionDescriptor, HttpStatusCode> StatusCodeFactory { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="ContentFactory"/> cannot be null - or -
    /// <see cref="ContentType"/> cannot be null - or -
    /// <see cref="StatusCodeFactory"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(ContentFactory == null);
        Validator.ThrowIfInvalidState(ContentType == null);
        Validator.ThrowIfInvalidState(StatusCodeFactory == null);
    }
}
