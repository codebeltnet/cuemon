using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Cuemon.Configuration;

namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// Configuration options for <see cref="UserAgentSentinelMiddleware"/> and related.
/// </summary>
public class UserAgentSentinelOptions : IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserAgentSentinelOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="UserAgentSentinelOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="AllowedUserAgents"/></term>
    ///         <description><c>new List{string}();</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="BadRequestMessage"/></term>
    ///         <description>The requirements of the request was not met.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ForbiddenMessage"/></term>
    ///         <description>The HTTP User-Agent specified was rejected.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="RequireUserAgentHeader"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ValidateUserAgentHeader"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ResponseHandler"/></term>
    ///         <description>A <see cref="HttpResponseMessage"/> initialized to either a HTTP status code 400 or 403 and a body of either <see cref="BadRequestMessage"/> or <see cref="ForbiddenMessage"/>.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="UseGenericResponse"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public UserAgentSentinelOptions()
    {
        BadRequestMessage = "The requirements of the request was not met.";
        ForbiddenMessage = "The User-Agent specified was rejected.";
        AllowedUserAgents = new List<string>();
        ResponseHandler = userAgent =>
        {
            var userAgentIsNullOrWhiteSpace = string.IsNullOrWhiteSpace(userAgent);
            var forbidden = !userAgentIsNullOrWhiteSpace &&
                            ValidateUserAgentHeader &&
                            AllowedUserAgents.Count > 0 &&
                            !AllowedUserAgents.Any(allowedUserAgent => userAgent.Equals(allowedUserAgent, StringComparison.OrdinalIgnoreCase));

            if (userAgentIsNullOrWhiteSpace || forbidden && UseGenericResponse)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(BadRequestMessage)
                };
            }

            if (forbidden)
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent(ForbiddenMessage)
                };
            }

            return null;
        };
    }

    /// <summary>
    /// Gets or sets the function delegate that configures the response in the form of a <see cref="HttpResponseMessage"/>.
    /// </summary>
    /// <value>The function delegate that configures the response in the form of a <see cref="HttpResponseMessage"/>.</value>
    public Func<string, HttpResponseMessage> ResponseHandler { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the produced <see cref="ResponseHandler"/> should be as neutral as possible.
    /// </summary>
    /// <value><c>true</c> if the produced <see cref="ResponseHandler"/> should be as neutral as possible; otherwise, <c>false</c>.</value>
    public bool UseGenericResponse { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a HTTP User-Agent header must be present in the request.
    /// </summary>
    /// <value><c>true</c> if the HTTP User-Agent header must be present in the request; otherwise, <c>false</c>.</value>
    public bool RequireUserAgentHeader { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a HTTP User-Agent header must be validated against <see cref="AllowedUserAgents"/>.
    /// </summary>
    /// <value><c>true</c> if the HTTP User-Agent header must be validated against <see cref="AllowedUserAgents"/>; otherwise, <c>false</c>.</value>
    public bool ValidateUserAgentHeader { get; set; }

    /// <summary>
    /// Gets or sets a list of whitelisted user agents.
    /// </summary>
    /// <value>A list of whitelisted user agents.</value>
    public IList<string> AllowedUserAgents { get; set; }

    /// <summary>
    /// Gets or sets the message of a request missing the requirements of a User-Agent header.
    /// </summary>
    /// <value>The message of a request missing the requirements of a User-Agent header.</value>
    public string BadRequestMessage { get; set; }

    /// <summary>
    /// Gets or sets the message of a request without a valid User-Agent header.
    /// </summary>
    /// <value>The message of a request without a valid User-Agent header.</value>
    public string ForbiddenMessage { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="ResponseHandler"/> cannot be null - or -
    /// <see cref="AllowedUserAgents"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(ResponseHandler == null);
        Validator.ThrowIfInvalidState(AllowedUserAgents == null);
    }
}
