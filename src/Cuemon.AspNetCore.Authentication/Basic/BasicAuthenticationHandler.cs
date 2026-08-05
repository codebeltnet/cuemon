using System.Collections.Generic;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication.Basic
{
    /// <summary>
    /// Provides a HTTP Basic Authentication implementation of <see cref="AuthenticationHandler{TOptions}"/> for ASP.NET Core.
    /// </summary>
    /// <seealso cref="AuthenticationHandler{TOptions}" />
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The monitor for the options instance.</param>
        /// <param name="logger">The <see cref="ILoggerFactory" />.</param>
        /// <param name="encoder">The <see cref="UrlEncoder" />.</param>
        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        /// <summary>
        /// Handle authenticate as an asynchronous operation.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.</returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Context.Items.TryAdd(nameof(BasicAuthenticationOptions), Options);

            if (!Authenticator.TryAuthenticate(Context, Options.RequireSecureConnection, BasicAuthenticationMiddleware.AuthorizationHeaderParser, BasicAuthenticationMiddleware.TryAuthenticate, out var principal))
            {
                var unathorized = new UnauthorizedException(Options.UnauthorizedMessage, principal.Failure);
                return Task.FromResult(AuthenticateResult.Fail(unathorized));
            }

            var ticket = new AuthenticationTicket(principal.Result, BasicAuthorizationHeader.Scheme);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        /// <summary>
        /// Handle challenge as an asynchronous operation.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            AuthenticationHandlerFeature.Set(await HandleAuthenticateOnceSafeAsync().ConfigureAwait(false), Context); // so annoying that Microsoft does not propagate AuthenticateResult properly - other have noticed as well: https://github.com/dotnet/aspnetcore/issues/44100
            Decorator.Enclose(Response.Headers).TryAdd(HeaderNames.WWWAuthenticate, string.Create(CultureInfo.InvariantCulture, $"{BasicAuthorizationHeader.Scheme} realm=\"{Options.Realm}\""));
            await base.HandleChallengeAsync(properties).ConfigureAwait(false);
        }
    }
}
