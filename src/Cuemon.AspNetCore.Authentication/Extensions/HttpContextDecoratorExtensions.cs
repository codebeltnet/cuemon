using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Authentication;
internal static class HttpContextDecoratorExtensions
{
    internal static async Task InvokeUnauthorizedExceptionAsync<TOptions>(this IDecorator<HttpContext> decorator, TOptions options, Exception reason, Action<HttpContext> wwwAuthenticateFactory = null) where TOptions : AuthenticationOptions
    {
        wwwAuthenticateFactory?.Invoke(decorator.Inner);
        var message = options.ResponseHandler?.Invoke();
        if (message != null)
        {
            throw Decorator.Enclose(new UnauthorizedException(await message.Content.ReadAsStringAsync().ConfigureAwait(false), reason))
                .AddResponseHeaders(decorator.Inner.Response.Headers)
                .AddResponseHeaders(message.Headers).Inner;
        }
    }
}
