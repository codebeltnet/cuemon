using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Authentication;
/// <summary>
/// Provides an opinionated implementation of <see cref="IAuthorizationMiddlewareResultHandler"/> that is optimized to deliver meaningful responses based on HTTP content negotiation.
/// </summary>
/// <remarks>This implementation relies on <see cref="IAuthenticateResultFeature"/> to provide details about AuthN/AuthZ related issues.</remarks>
public class AuthorizationResponseHandler : Configurable<AuthorizationResponseHandlerOptions>, IAuthorizationMiddlewareResultHandler
{
    private readonly ILogger<AuthorizationResponseHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationResponseHandler"/> class.
    /// </summary>
    /// <param name="logger">The dependency injected <see cref="ILogger{TCategoryName}"/>.</param>
    /// <param name="options">The <see cref="AuthorizationResponseHandlerOptions" /> which may be configured.</param>
    public AuthorizationResponseHandler(ILogger<AuthorizationResponseHandler> logger, IOptions<AuthorizationResponseHandlerOptions> options) : base(options?.Value)
    {
        _logger = logger;
    }

    /// <summary>
    /// Evaluates the authorization requirement and processes the authorization response.
    /// </summary>
    /// <param name="next">The next middleware in the application pipeline.</param>
    /// <param name="context">The <see cref="HttpContext"/> of the current request.</param>
    /// <param name="policy">The <see cref="AuthorizationPolicy" /> for the resource.</param>
    /// <param name="authorizeResult">The <see cref="PolicyAuthorizationResult"/> of authorization.</param>
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Succeeded)
        {
            await next(context).ConfigureAwait(false);
            return;
        }

        await HandleChainedDependenciesAsync(context, policy, authorizeResult).ConfigureAwait(false);

        try
        {
            Exception failure = null;
            if (authorizeResult.Challenged)
            {
                var authenticationFeature = context.Features.Get<IAuthenticateResultFeature>();
                failure = authenticationFeature?.AuthenticateResult?.Failure ?? throw new InvalidOperationException($"Unable to retrieve an implementation of {nameof(IAuthenticateResultFeature)} or an associated {nameof(IAuthenticateResultFeature.AuthenticateResult)} with a proper set {nameof(IAuthenticateResultFeature.AuthenticateResult.Failure)}.");
            }
            else if (authorizeResult.Forbidden)
            {
                failure = Options.AuthorizationFailureHandler.Invoke(authorizeResult.AuthorizationFailure);
            }

            var exceptionDescriptor = new HttpExceptionDescriptor(failure);
            if (authorizeResult.Challenged) { exceptionDescriptor.StatusCode = StatusCodes.Status401Unauthorized; } // do not allow anything but 401 for challenged (WWW-Authenticate)
            var handlers = context.RequestServices.GetExceptionResponseFormatters().SelectExceptionDescriptorHandlers();
            var accepts = context.Request.AcceptMimeTypesOrderedByQuality();
            foreach (var accept in accepts)
            {
                var handler = handlers.FirstOrDefault(rh => rh.ContentType.MediaType != null && rh.ContentType.MediaType.Equals(accept, StringComparison.OrdinalIgnoreCase));
                if (handler != null)
                {
                    await WriteResponseAsync(context, handler, exceptionDescriptor, Options.CancellationToken).ConfigureAwait(false);
                    return;
                }
            }

            var fallback = HttpExceptionDescriptorResponseHandler.CreateDefaultFallbackHandler(context.RequestServices.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value.SensitivityDetails);
            await WriteResponseAsync(context, fallback, exceptionDescriptor, Options.CancellationToken).ConfigureAwait(false); // fallback in case no match from Accept header
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unable to deliver a meaningful response based on HTTP content negotiation; reverting to {nameof(Options.FallbackResponseHandler)}.");
            await Options.FallbackResponseHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }

    private static async Task HandleChainedDependenciesAsync(HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Challenged)
        {
            if (policy.AuthenticationSchemes.Count > 0)
            {
                foreach (var scheme in policy.AuthenticationSchemes)
                {
                    await context.ChallengeAsync(scheme);
                }
            }
            else
            {
                await context.ChallengeAsync();
            }
        }
        else if (authorizeResult.Forbidden)
        {
            if (policy.AuthenticationSchemes.Count > 0)
            {
                foreach (var scheme in policy.AuthenticationSchemes)
                {
                    await context.ForbidAsync(scheme);
                }
            }
            else
            {
                await context.ForbidAsync();
            }
        }
    }

    private static Task WriteResponseAsync(HttpContext context, HttpExceptionDescriptorResponseHandler handler, HttpExceptionDescriptor exceptionDescriptor, CancellationToken ct = default)
    {
        return Decorator.Enclose(context).WriteExceptionDescriptorResponseAsync(handler, exceptionDescriptor, ct);
    }
}
