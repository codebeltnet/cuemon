using Cuemon.Data.Integrity;
using Microsoft.Net.Http.Headers;

namespace Cuemon.Extensions.AspNetCore.Data.Integrity;
/// <summary>
/// Extension methods for the <see cref="CacheValidator"/> class.
/// </summary>
public static class CacheValidatorExtensions
{
    /// <summary>
    /// Creates an <see cref="EntityTagHeaderValue"/> from the specified <paramref name="validator"/>.
    /// </summary>
    /// <param name="validator">The validator to extend.</param>
    /// <returns>An <see cref="EntityTagHeaderValue"/> that is initiated with a hexadecimal representation of <see cref="ChecksumBuilder.Checksum"/> and a value that indicates if the tag is weak.</returns>
    public static EntityTagHeaderValue ToEntityTagHeaderValue(this CacheValidator validator)
    {
        Validator.ThrowIfNull(validator);
        return validator.ToEntityTagHeaderValue(validator.Validation != EntityDataIntegrityValidation.Strong);
    }
}
