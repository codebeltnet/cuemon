using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Provides a combined default implementation of <see cref="IAuthenticateResultFeature"/> and <see cref="IHttpAuthenticationFeature"/> so that <see cref="AuthenticateResult"/> and <see cref="User"/> is consistent with each other.
    /// </summary>
    /// <remarks>Inspiration/cloned from https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/Core/src/AuthenticationFeatures.cs</remarks>
    /// <seealso cref="IAuthenticateResultFeature" />
    /// <seealso cref="IHttpAuthenticationFeature" />
    public class AuthenticationHandlerFeature : IAuthenticateResultFeature, IHttpAuthenticationFeature
    {
        /// <summary>
        /// Provides a convenient and consistent way of propagating HTTP features for <see cref="IAuthenticateResultFeature"/> and <see cref="IHttpAuthenticationFeature"/>.
        /// </summary>
        /// <param name="result">The <see cref="Microsoft.AspNetCore.Authentication.AuthenticateResult"/> to propagate.</param>
        /// <param name="context">The <see cref="HttpContext"/> to use as propagation channel.</param>
        public static void Set(AuthenticateResult result, HttpContext context)
        {
            var authenticationHandlerFeature = new AuthenticationHandlerFeature(result);
            context.Features.Set<IAuthenticateResultFeature>(authenticationHandlerFeature);
            context.Features.Set<IHttpAuthenticationFeature>(authenticationHandlerFeature);
        }

        private ClaimsPrincipal _user;
        private AuthenticateResult _result;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationHandlerFeature"/> class.
        /// </summary>
        /// <param name="result">The <see cref="Microsoft.AspNetCore.Authentication.AuthenticateResult"/> to propagate.</param>
        public AuthenticationHandlerFeature(AuthenticateResult result)
        {
            AuthenticateResult = result;
        }

        /// <summary>
        /// The <see cref="IAuthenticateResultFeature.AuthenticateResult" /> from the authorization middleware.
        /// </summary>
        /// <value>The <see cref="Microsoft.AspNetCore.Authentication.AuthenticateResult"/> to propagate.</value>
        public AuthenticateResult AuthenticateResult
        {
            get => _result;
            set
            {
                _result = value;
                _user = _result?.Principal;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ClaimsPrincipal" /> associated with the HTTP request.
        /// </summary>
        /// <value>The <see cref="ClaimsPrincipal" /> associated with the HTTP request.</value>
        public ClaimsPrincipal User
        {
            get => _user;
            set
            {
                _user = value;
                _result = null;
            }
        }
    }
}
