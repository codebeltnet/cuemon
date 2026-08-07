using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Data.Integrity;
using Microsoft.Net.Http.Headers;

namespace Cuemon.Extensions.AspNetCore.Data.Integrity;
/// <summary>
/// Extension methods for the <see cref="ChecksumBuilder"/> class.
/// </summary>
public static class ChecksumBuilderExtensions
{
    /// <summary>
    /// Creates an <see cref="EntityTagHeaderValue"/> from the specified <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="isWeak">A value that indicates if this entity-tag header is a weak validator.</param>
    /// <returns>An <see cref="EntityTagHeaderValue"/> that is initiated with a hexadecimal representation of <see cref="ChecksumBuilder.Checksum"/> and a value that indicates if the tag is weak.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    public static EntityTagHeaderValue ToEntityTagHeaderValue(this ChecksumBuilder builder, bool isWeak = false)
    {
        Validator.ThrowIfNull(builder);
        return Decorator.Enclose(builder).ToEntityTagHeaderValue(isWeak);
    }
}
