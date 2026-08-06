using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Authentication.Assets;
public class ExceptionMiddleware : Middleware
{
    public ExceptionMiddleware(RequestDelegate next) : base(next)
    {
    }

    public override async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (Exception exception)
        {
            if (exception is ArgumentException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            throw;
        }
    }
}

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return MiddlewareBuilderFactory.UseMiddleware<ExceptionMiddleware>(builder);
    }
}
