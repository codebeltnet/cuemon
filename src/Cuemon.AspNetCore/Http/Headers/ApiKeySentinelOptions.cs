using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Cuemon.Configuration;
using Cuemon.Net.Http;

namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// Configuration options for <see cref="ApiKeySentinelMiddleware"/> and related.
/// </summary>
public class ApiKeySentinelOptions : IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeySentinelOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="UserAgentSentinelOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="HeaderName"/></term>
    ///         <description><see cref="HttpHeaderNames.XApiKey"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="AllowedKeys"/></term>
    ///         <description><c>new List{string}();</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="GenericClientMessage"/></term>
    ///         <description>The requirements of the request was not met.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="GenericClientStatusCode"/></term>
    ///         <description><see cref="HttpStatusCode.BadRequest"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ForbiddenMessage"/></term>
    ///         <description>The API key specified was rejected.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ResponseHandler"/></term>
    ///         <description>A <see cref="HttpResponseMessage"/> initialized to either a HTTP status code 400 or 403 and a body of either <see cref="GenericClientMessage"/> or <see cref="ForbiddenMessage"/>.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="UseGenericResponse"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public ApiKeySentinelOptions()
    {
        HeaderName = HttpHeaderNames.XApiKey;
        GenericClientStatusCode = HttpStatusCode.BadRequest;
        GenericClientMessage = "The requirements of the request was not met.";
        ForbiddenMessage = "The API key specified was rejected.";
        AllowedKeys = new List<string>();
        ResponseHandler = apiKey =>
        {
            var apiKeyIsNullOrWhiteSpace = string.IsNullOrWhiteSpace(apiKey);
            var forbidden = !apiKeyIsNullOrWhiteSpace &&
                            AllowedKeys.Count > 0 &&
                            !AllowedKeys.Any(allowedApiKey => apiKey.Equals(allowedApiKey, StringComparison.OrdinalIgnoreCase));

            if (apiKeyIsNullOrWhiteSpace || (forbidden && UseGenericResponse))
            {
                return new HttpResponseMessage(GenericClientStatusCode)
                {
                    Content = new StringContent(GenericClientMessage)
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
    /// Gets or sets the name of the API key HTTP header.
    /// </summary>
    /// <value>The name of the API key HTTP header.</value>
    public string HeaderName { get; set; }

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
    /// Gets or sets the generic status code of a request without a valid key in <see cref="AllowedKeys"/>.
    /// </summary>
    /// <value>The generic status code of a request without a valid key in <see cref="AllowedKeys"/>.</value>
    public HttpStatusCode GenericClientStatusCode { get; set; }

    /// <summary>
    /// Gets or sets the generic message of a request without a valid key in <see cref="AllowedKeys"/>.
    /// </summary>
    /// <value>The generic message of a request without a valid key in <see cref="AllowedKeys"/>.</value>
    public string GenericClientMessage { get; set; }


    /// <summary>
    /// Gets or sets a list of whitelisted API keys.
    /// </summary>
    /// <value>A list of whitelisted API keys.</value>
    public IList<string> AllowedKeys { get; set; }

    /// <summary>
    /// Gets or sets the message of a request without a valid <see cref="HeaderName"/>.
    /// </summary>
    /// <value>The message of a request without a valid <see cref="HeaderName"/>.</value>
    public string ForbiddenMessage { get; set; }


    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="HeaderName"/> cannot be null, empty or consist only of white-space characters - or -
    /// <see cref="ResponseHandler"/> cannot be null - or -
    /// <see cref="AllowedKeys"/> cannot be null - or -
    /// <see cref="GenericClientStatusCode"/> is not within the allowed range of an HTTP Client Error Status Code (400-499).
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName));
        Validator.ThrowIfInvalidState(ResponseHandler == null);
        Validator.ThrowIfInvalidState(AllowedKeys == null);
        Validator.ThrowIfInvalidState((int)GenericClientStatusCode < 400 || (int)GenericClientStatusCode > 499);
    }
}
