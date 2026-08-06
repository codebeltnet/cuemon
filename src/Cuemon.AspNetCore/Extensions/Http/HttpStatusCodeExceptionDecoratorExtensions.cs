using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// Extension methods for the <see cref="HttpStatusCodeException"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class HttpStatusCodeExceptionDecoratorExtensions
{
    /// <summary>
    /// Adds all non-existing <paramref name="headers"/> to the enclosed <see cref="HttpStatusCodeException"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="headers">The <see cref="IHeaderDictionary"/> to populate into the enclosed <see cref="HttpStatusCodeException"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="headers"/> cannot be null.
    /// </exception>
    public static IDecorator<T> AddResponseHeaders<T>(this IDecorator<T> decorator, IHeaderDictionary headers) where T : HttpStatusCodeException
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(headers);
        foreach (var header in headers)
        {
            if (!decorator.Inner.Headers.Contains(header))
            {
                decorator.Inner.Headers.Add(header);
            }
        }
        return decorator;
    }

    /// <summary>
    /// Adds all non-existing <paramref name="headers"/> to the enclosed <see cref="HttpStatusCodeException"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="headers">The <see cref="HttpResponseHeaders"/> to populate into the enclosed <see cref="HttpStatusCodeException"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="headers"/> cannot be null.
    /// </exception>
    public static IDecorator<T> AddResponseHeaders<T>(this IDecorator<T> decorator, HttpResponseHeaders headers) where T : HttpStatusCodeException
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(headers);
        foreach (var header in headers)
        {
            if (!decorator.Inner.Headers.ContainsKey(header.Key))
            {
                decorator.Inner.Headers.Add(new KeyValuePair<string, StringValues>(header.Key, new StringValues(header.Value.ToArray())));
            }
        }
        return decorator;
    }
}
