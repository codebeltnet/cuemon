using System;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Provides information about an <see cref="Exception" />, in a developer friendly way, optimized for open- and otherwise public application programming interfaces (API).
/// Implements the <see cref="ExceptionDescriptor" />
/// </summary>
/// <seealso cref="ExceptionDescriptor" />
public class HttpExceptionDescriptor : ExceptionDescriptor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpExceptionDescriptor" /> class.
    /// </summary>
    /// <param name="failure">The <see cref="Exception"/> that caused the current failure.</param>
    /// <param name="statusCode">The status code of the HTTP request.</param>
    /// <param name="code">The error code that uniquely identifies the type of failure.</param>
    /// <param name="message">The message that explains the reason for the failure.</param>
    /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="HttpExceptionDescriptor"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Parameter</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><paramref name="statusCode"/></term>
    ///         <description><c>StatusCodes.Status500InternalServerError</c></description>
    ///     </item>
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
    public HttpExceptionDescriptor(Exception failure, int statusCode = StatusCodes.Status500InternalServerError, string code = null, string message = null, Uri helpLink = null)
        : base(
            failure,
            Validator.CheckParameter(() =>
            {
                if (failure is HttpStatusCodeException httpException)
                {
                    statusCode = httpException.StatusCode;
                }
                return code ?? ReasonPhrases.GetReasonPhrase(statusCode);
            }),
            message ?? failure.Message, helpLink)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    /// Gets or sets the HTTP status code of the service request the caller made.
    /// </summary>
    /// <value>The HTTP status code of the service request the caller made.</value>
    public int StatusCode { get; set; }

    /// <summary>  
    /// Gets or sets the URI that identifies the specific occurrence of the problem.  
    /// </summary>  
    /// <value>The URI that identifies the specific occurrence of the problem.</value>  
    public Uri Instance { get; set; }

    /// <summary>
    /// Gets or sets the request identifier that uniquely identifies the service request the caller made.
    /// </summary>
    /// <value>An identifier that uniquely identifies the service request the caller made.</value>
    public string RequestId { get; set; }

    /// <summary>
    /// Gets or sets the correlation identifier that allows a reference to a particular transaction or event chain the caller made.
    /// </summary>
    /// <value>An identifier that allows a reference to a particular transaction or event chain the caller made.</value>
    public string CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the trace identifier that uniquely identifies the trace of the service request the caller made.
    /// </summary>
    /// <value>A trace identifier that uniquely identifies the trace of the service request the caller made.</value>
    public string TraceId { get; set; }
}
