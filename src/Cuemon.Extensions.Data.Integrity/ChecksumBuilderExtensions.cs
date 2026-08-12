using Cuemon.Data.Integrity;

namespace Cuemon.Extensions.Data.Integrity;
/// <summary>
/// Extension methods for the <see cref="ChecksumBuilder"/> class.
/// </summary>
public static class ChecksumBuilderExtensions
{
    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="cb">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="double"/> value containing a checksum of the additional data this instance must represent.</param>
    /// <returns>An updated instance of the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
    public static T CombineWith<T>(this T cb, double additionalChecksum) where T : ChecksumBuilder
    {
        return Decorator.Enclose(cb).CombineWith(additionalChecksum);
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="cb">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="additionalChecksum">An <see cref="short"/> value containing a checksum of the additional data this instance must represent.</param>
    /// <returns>An updated instance of the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
    public static T CombineWith<T>(this T cb, short additionalChecksum) where T : ChecksumBuilder
    {
        return Decorator.Enclose(cb).CombineWith(additionalChecksum);
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="cb">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="string"/> value containing a checksum of the additional data this instance must represent.</param>
    /// <returns>An updated instance of the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
    public static T CombineWith<T>(this T cb, string additionalChecksum) where T : ChecksumBuilder
    {
        return Decorator.Enclose(cb).CombineWith(additionalChecksum);
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="cb">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="additionalChecksum">An <see cref="int"/> value containing a checksum of the additional data this instance must represent.</param>
    /// <returns>An updated instance of the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
    public static T CombineWith<T>(this T cb, int additionalChecksum) where T : ChecksumBuilder
    {
        return Decorator.Enclose(cb).CombineWith(additionalChecksum);
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="cb">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="additionalChecksum">An <see cref="long"/> value containing a checksum of the additional data this instance must represent.</param>
    /// <returns>An updated instance of the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
    public static T CombineWith<T>(this T cb, long additionalChecksum) where T : ChecksumBuilder
    {
        return Decorator.Enclose(cb).CombineWith(additionalChecksum);
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="cb">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="float"/> value containing a checksum of the additional data this instance must represent.</param>
    /// <returns>An updated instance of the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
    public static T CombineWith<T>(this T cb, float additionalChecksum) where T : ChecksumBuilder
    {
        return Decorator.Enclose(cb).CombineWith(additionalChecksum);
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="cb">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="additionalChecksum">An <see cref="ushort"/> value containing a checksum of the additional data this instance must represent.</param>
    /// <returns>An updated instance of the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
    public static T CombineWith<T>(this T cb, ushort additionalChecksum) where T : ChecksumBuilder
    {
        return Decorator.Enclose(cb).CombineWith(additionalChecksum);
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="cb">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="additionalChecksum">An <see cref="uint"/> value containing a checksum of the additional data this instance must represent.</param>
    /// <returns>An updated instance of the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
    public static T CombineWith<T>(this T cb, uint additionalChecksum) where T : ChecksumBuilder
    {
        return Decorator.Enclose(cb).CombineWith(additionalChecksum);
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="cb">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="additionalChecksum">An <see cref="ulong"/> value containing a checksum of the additional data this instance must represent.</param>
    /// <returns>An updated instance of the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
    public static T CombineWith<T>(this T cb, ulong additionalChecksum) where T : ChecksumBuilder
    {
        return Decorator.Enclose(cb).CombineWith(additionalChecksum);
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum" /> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="cb">The <see cref="ChecksumBuilder"/> to extend.</param>
    /// <param name="additionalChecksum">An array of bytes containing a checksum of the additional data this instance must represent.</param>
    /// <returns>An updated instance of the specified <paramref name="cb"/> of <typeparamref name="T"/>.</returns>
    public static T CombineWith<T>(this T cb, byte[] additionalChecksum) where T : ChecksumBuilder
    {
        return Decorator.Enclose(cb).CombineWith(additionalChecksum);
    }
}
