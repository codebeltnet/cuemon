using System;
using System.Globalization;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Collections.Generic;
using Cuemon.Data.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// Extension methods for the <see cref="HttpResponse"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class HttpResponseDecoratorExtensions
{
    /// <summary>
    /// Attempts to add or update the enclosed <see cref="HttpResponse"/> of the <paramref name="decorator"/> with the necessary HTTP response headers needed to provide entity tag header information.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
    /// <param name="builder">A <see cref="ChecksumBuilder"/> that represents the integrity of the client.</param>
    /// <param name="isWeak">A value that indicates if this entity-tag header is a weak validator.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="request"/> cannot be null -or-
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    public static void AddOrUpdateEntityTagHeader(this IDecorator<HttpResponse> decorator, HttpRequest request, ChecksumBuilder builder, bool isWeak = false)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(request);
        Validator.ThrowIfNull(builder);
        builder = Decorator.Enclose(builder).CombineWith(request.Headers[HeaderNames.Accept]);
        if (Decorator.Enclose(decorator.Inner.StatusCode).IsSuccessStatusCode() && Decorator.Enclose(request).IsClientSideResourceCached(builder)) { decorator.Inner.StatusCode = StatusCodes.Status304NotModified; }
        Decorator.Enclose(decorator.Inner.Headers).AddOrUpdate(HeaderNames.ETag, new StringValues(Decorator.Enclose(builder).ToEntityTagHeaderValue(isWeak).ToString()));
    }

    /// <summary>
    /// Attempts to add or update the enclosed <see cref="HttpResponse"/> of the <paramref name="decorator"/> with the necessary HTTP response headers needed to provide last-modified information.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
    /// <param name="lastModified">A value that represents when the resource was either created or last modified.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="request"/> cannot be null.
    /// </exception>
    public static void AddOrUpdateLastModifiedHeader(this IDecorator<HttpResponse> decorator, HttpRequest request, DateTime lastModified)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(request);
        if (Decorator.Enclose(decorator.Inner.StatusCode).IsSuccessStatusCode() && Decorator.Enclose(request).IsClientSideResourceCached(lastModified)) { decorator.Inner.StatusCode = StatusCodes.Status304NotModified; }
        Decorator.Enclose(decorator.Inner.Headers).AddOrUpdate(HeaderNames.LastModified, new StringValues(lastModified.ToUniversalTime().ToString("R", DateTimeFormatInfo.InvariantInfo)));
    }
}
