using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Http;
using Cuemon.Data.Integrity;
using Cuemon.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Cuemon.Extensions.AspNetCore.Http;
/// <summary>
/// Extension methods for the <see cref="HttpRequest"/> class.
/// </summary>
public static class HttpRequestExtensions
{

    /// <summary>
    /// Returns the MIME type types from the HTTP Accept header of the <paramref name="request"/>, ordered by their quality values, e.g., preferred MIME types are first.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequest"/> to extend.</param>
    /// <returns>
    /// A sequence of strings representing the MIME types from the HTTP Accept header of the <paramref name="request"/>, ordered by their quality values.
    /// </returns>
    public static IEnumerable<string> AcceptMimeTypesOrderedByQuality(this HttpRequest request)
    {
        return request.Headers[HttpHeaderNames.Accept]
            .OrderByDescending(accept =>
            {
                var values = accept.Split(';').Select(raw => raw.Trim());
                return values.FirstOrDefault(quality => quality.StartsWith("q=", StringComparison.OrdinalIgnoreCase)) ?? "q=0.0";
            })
            .Select(accept => accept.Split(';')[0])
            .ToList();
    }

    /// <summary>
    /// Determines whether the specified <paramref name="request"/> is served by either a GET or a HEAD method.
    /// </summary>
    /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
    /// <returns><c>true</c> if the specified <paramref name="request"/> is served by either a GET or a HEAD method; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="request"/> cannot be null.
    /// </exception>
    public static bool IsGetOrHeadMethod(this HttpRequest request)
    {
        Validator.ThrowIfNull(request);
        return Decorator.Enclose(request).IsGetOrHeadMethod();
    }

    /// <summary>
    /// Determines whether a cached version of the requested resource is found client-side using the If-None-Match HTTP header.
    /// </summary>
    /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
    /// <param name="builder">A <see cref="ChecksumBuilder"/> that represents the integrity of the client.</param>
    /// <returns>
    ///     <c>true</c> if a cached version of the requested content is found client-side; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="request"/> cannot be null -or
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    public static bool IsClientSideResourceCached(this HttpRequest request, ChecksumBuilder builder)
    {
        Validator.ThrowIfNull(request);
        return Decorator.Enclose(request).IsClientSideResourceCached(builder);
    }

    /// <summary>
    /// Determines whether a cached version of the requested resource is found client-side using the If-Modified-Since HTTP header.
    /// </summary>
    /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
    /// <param name="lastModified">A <see cref="DateTime"/> value that represents the modification date of the content.</param>
    /// <returns>
    ///     <c>true</c> if a cached version of the requested content is found client-side; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="request"/> cannot be null.
    /// </exception>
    public static bool IsClientSideResourceCached(this HttpRequest request, DateTime lastModified)
    {
        Validator.ThrowIfNull(request);
        return Decorator.Enclose(request).IsClientSideResourceCached(lastModified);
    }
}
