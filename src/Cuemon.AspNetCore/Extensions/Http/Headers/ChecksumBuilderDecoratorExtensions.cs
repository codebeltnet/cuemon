using System;
using Cuemon.Data.Integrity;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// Extension methods for the <see cref="ChecksumBuilder"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class ChecksumBuilderDecoratorExtensions
{
    /// <summary>
    /// Creates an <see cref="EntityTagHeaderValue"/> from the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="isWeak">A value that indicates if this entity-tag header is a weak validator.</param>
    /// <returns>An <see cref="EntityTagHeaderValue"/> that is initiated with a hexadecimal representation of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> and a value that indicates if the tag is weak.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static EntityTagHeaderValue ToEntityTagHeaderValue(this IDecorator<ChecksumBuilder> decorator, bool isWeak = false)
    {
        Validator.ThrowIfNull(decorator);
        return new EntityTagHeaderValue(string.Concat("\"", decorator.Inner.Checksum.ToHexadecimalString(), "\""), isWeak);
    }
}
