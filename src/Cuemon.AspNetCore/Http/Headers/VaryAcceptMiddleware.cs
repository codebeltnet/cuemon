using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// Middleware that appends <c>Vary: Accept</c> to every response, signalling to HTTP
/// caches (and clients) that the representation varies based on the <c>Accept</c>
/// request header (RFC 9110 §12.5.5).
/// </summary>
public class VaryAcceptMiddleware : Middleware
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VaryAcceptMiddleware"/> class.
    /// </summary>
    /// <param name="next">
    /// The next <see cref="RequestDelegate"/> in the ASP.NET Core request processing pipeline.
    /// This delegate is invoked after this middleware has performed its work.
    /// </param>
    public VaryAcceptMiddleware(RequestDelegate next) : base(next)
    {
    }

    /// <summary>
    /// Invokes the middleware for the given <see cref="HttpContext"/>.
    /// Adds the <c>Vary: Accept</c> response header just before the response starts.
    /// </summary>
    /// <param name="context">The current <see cref="HttpContext"/>.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation of this middleware.
    /// The returned task completes when the remaining pipeline has finished processing.
    /// </returns>
    /// <remarks>
    /// The header is appended using <see cref="HttpResponse.OnStarting(Func{Task})"/>
    /// to ensure the header is present even if the response body is being streamed or the response
    /// was started by downstream middleware. This signals to intermediaries and clients that the
    /// response representation depends on the request's <c>Accept</c> header.
    /// </remarks>
    public override Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            var headers = context.Response.Headers;
            var existing = headers[HeaderNames.Vary].ToString();
            if (string.IsNullOrEmpty(existing))
            {
                headers[HeaderNames.Vary] = HeaderNames.Accept;
                return Task.CompletedTask;
            }
            foreach (var segment in existing.Split(','))
            {
                if (segment.Trim().Equals(HeaderNames.Accept, StringComparison.OrdinalIgnoreCase))
                {
                    return Task.CompletedTask;
                }
            }
            headers[HeaderNames.Vary] = existing + ", " + HeaderNames.Accept;
            return Task.CompletedTask;
        });

        return Next(context);
    }
}
