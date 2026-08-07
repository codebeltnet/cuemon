using System;
using System.Linq;
using Cuemon.Data.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// Extension methods for the <see cref="HttpRequest"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class HttpRequestDecoratorExtensions
{
    /// <summary>
    /// Determines whether the enclosed <see cref="HttpRequest"/> of the <paramref name="decorator"/> is served by either a GET or a HEAD method.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the enclosed <see cref="HttpRequest"/> of the <paramref name="decorator"/> is served by either a GET or a HEAD method; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsGetOrHeadMethod(this IDecorator<HttpRequest> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var method = decorator.Inner.Method;
        return HttpMethods.IsGet(method) || HttpMethods.IsHead(method);
    }

    /// <summary>
    /// Determines whether a cached version of the enclosed <see cref="HttpRequest"/> of the <paramref name="decorator"/> is found client-side using the If-None-Match HTTP header.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="builder">A <see cref="ChecksumBuilder"/> that represents the integrity of the client.</param>
    /// <returns><c>true</c> if a cached version of the enclosed <see cref="HttpRequest"/> of the <paramref name="decorator"/> is found client-side; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    public static bool IsClientSideResourceCached(this IDecorator<HttpRequest> decorator, ChecksumBuilder builder)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(builder);
        var headers = new RequestHeaders(decorator.Inner.Headers);
        if (headers.IfNoneMatch != null)
        {
            var clientSideEntityTagHeader = headers.IfNoneMatch.FirstOrDefault();
            var clientSideEntityTag = clientSideEntityTagHeader == null ? "" : clientSideEntityTagHeader.Tag.Value;
            var indexOfStartQuote = clientSideEntityTag.IndexOf('"');
            var indexOfEndQuote = clientSideEntityTag.LastIndexOf('"');
            if (indexOfStartQuote == 0 &&
                (indexOfEndQuote > 2 && indexOfEndQuote == clientSideEntityTag.Length - 1))
            {
                clientSideEntityTag = clientSideEntityTag.Remove(indexOfEndQuote, 1);
                clientSideEntityTag = clientSideEntityTag.Remove(indexOfStartQuote, 1);
            }
            return builder.Checksum.ToHexadecimalString().Equals(clientSideEntityTag, StringComparison.Ordinal);
        }
        return false;
    }

    /// <summary>
    /// Determines whether a cached version of the enclosed <see cref="HttpRequest"/> of the <paramref name="decorator"/> is found client-side using the If-Modified-Since HTTP header.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="lastModified">A <see cref="DateTime"/> value that represents the modification date of the content.</param>
    /// <returns><c>true</c> if a cached version of the enclosed <see cref="HttpRequest"/> of the <paramref name="decorator"/> is found client-side; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsClientSideResourceCached(this IDecorator<HttpRequest> decorator, DateTime lastModified)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(lastModified);
        var headers = new RequestHeaders(decorator.Inner.Headers);
        var adjustedLastModified = Tweaker.Adjust(lastModified, o => new DateTime(o.Year, o.Month, o.Day, o.Hour, o.Minute, o.Second, DateTimeKind.Utc)); // make sure, that modified has the same format as the if-modified-since header
        var ifModifiedSince = headers.IfModifiedSince?.UtcDateTime;
        return (adjustedLastModified != DateTime.MinValue) && (ifModifiedSince.HasValue && ifModifiedSince.Value.ToUniversalTime() >= adjustedLastModified);
    }
}
