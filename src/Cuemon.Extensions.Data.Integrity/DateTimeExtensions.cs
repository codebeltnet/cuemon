using System;
using Cuemon.Data.Integrity;
using Cuemon.Security;

namespace Cuemon.Extensions.Data.Integrity;
/// <summary>
/// Extension methods for the <see cref="DateTime"/> struct.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Returns a <see cref="CacheValidator"/> from the specified parameters.
    /// </summary>
    /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
    /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
    /// <param name="hashFactory">The function delegate that is invoked to produce the <see cref="HashResult" />. Default is <see cref="HashFactory.CreateFnv128"/>.</param>
    /// <param name="method">A <see cref="EntityDataIntegrityMethod"/> enumeration value that indicates how a checksum is manipulated. Default is <see cref="EntityDataIntegrityMethod.Unaltered"/>.</param>
    /// <returns>A <see cref="CacheValidator"/> that represents the integrity of the specified parameters.</returns>
    public static CacheValidator GetCacheValidator(this DateTime created, DateTime? modified = null, Func<Hash> hashFactory = null, EntityDataIntegrityMethod method = EntityDataIntegrityMethod.Unaltered)
    {
        hashFactory ??= () => HashFactory.CreateFnv128();
        return new CacheValidator(new EntityInfo(created, modified), hashFactory, method);
    }

    /// <summary>
    /// Returns a <see cref="CacheValidator"/> from the specified parameters.
    /// </summary>
    /// <param name="created">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was first created.</param>
    /// <param name="modified">A <see cref="DateTime"/> value for when data this <see cref="CacheValidator"/> represents was last modified.</param>
    /// <param name="checksum">An array of bytes containing a checksum of the data this <see cref="CacheValidator"/> represents.</param>
    /// <param name="validation">A <see cref="EntityDataIntegrityValidation"/> enumeration value that indicates the validation strength of the specified <paramref name="checksum"/>. Default is <see cref="EntityDataIntegrityValidation.Weak"/>.</param>
    /// <param name="hashFactory">The function delegate that is invoked to produce the <see cref="HashResult" />. Default is <see cref="HashFactory.CreateFnv128"/>.</param>
    /// <param name="method">A <see cref="EntityDataIntegrityMethod"/> enumeration value that indicates how a checksum is manipulated. Default is <see cref="EntityDataIntegrityMethod.Unaltered"/>.</param>
    /// <returns>A <see cref="CacheValidator"/> that represents the integrity of the specified parameters.</returns>
    public static CacheValidator GetCacheValidator(this DateTime created, DateTime modified, byte[] checksum, EntityDataIntegrityValidation validation = EntityDataIntegrityValidation.Weak, Func<Hash> hashFactory = null, EntityDataIntegrityMethod method = EntityDataIntegrityMethod.Unaltered)
    {
        hashFactory ??= () => HashFactory.CreateFnv128();
        return new CacheValidator(new EntityInfo(created, modified, checksum, validation), hashFactory, method);
    }
}
