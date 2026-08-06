using System;
using System.Net;
using System.Net.Http;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Authentication;

namespace Cuemon.AspNetCore.Authentication;
/// <summary>
/// Base options for all authentication middleware.
/// </summary>
/// <seealso cref="AuthenticationSchemeOptions"/>
public abstract class AuthenticationOptions : AuthenticationSchemeOptions, IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AuthenticationOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="ResponseHandler"/></term>
    ///         <description><c>() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent(UnauthorizedMessage) };</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="RequireSecureConnection"/></term>
    ///         <description><c>true</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="UnauthorizedMessage"/></term>
    ///         <description>The request has not been applied because it lacks valid authentication credentials for the target resource.</description>
    ///     </item>
    /// </list>
    /// </remarks>
    protected AuthenticationOptions()
    {
        UnauthorizedMessage = "The request has not been applied because it lacks valid authentication credentials for the target resource.";
        ResponseHandler = () => new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent(UnauthorizedMessage) };
        RequireSecureConnection = true;
    }

    /// <summary>
    /// Gets or sets a value indicating whether a HTTP connection is required to use secure sockets (that is, HTTPS).
    /// </summary>
    /// <value><c>true</c> if the HTTP connection is required to use secure sockets (that is, HTTPS); otherwise, <c>false</c>.</value>
    public bool RequireSecureConnection { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that configures the unauthorized response in the form of a <see cref="HttpResponseMessage"/>.
    /// </summary>
    /// <value>The function delegate that configures the unauthorized response in the form of a <see cref="HttpResponseMessage"/>.</value>
    public Func<HttpResponseMessage> ResponseHandler { get; set; }

    /// <summary>
    /// Gets or sets the message of an unauthorized request.
    /// </summary>
    /// <value>The message of an unauthorized request.</value>
    public string UnauthorizedMessage { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <seealso cref="UnauthorizedMessage"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public virtual void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(UnauthorizedMessage == null);
    }
}
