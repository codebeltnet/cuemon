using System;

namespace Cuemon.Data.Integrity;
/// <summary>
/// Extension methods for the <see cref="ChecksumBuilder"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class ChecksumBuilderDecoratorExtensions
{
    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="double"/> value containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
    /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static T CombineWith<T>(this IDecorator<T> decorator, double additionalChecksum) where T : ChecksumBuilder
    {
        return CombineWith(decorator, Convertible.GetBytes(additionalChecksum));
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="short"/> value containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
    /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static T CombineWith<T>(this IDecorator<T> decorator, short additionalChecksum) where T : ChecksumBuilder
    {
        return CombineWith(decorator, Convertible.GetBytes(additionalChecksum));
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="string"/> containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
    /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static T CombineWith<T>(this IDecorator<T> decorator, string additionalChecksum) where T : ChecksumBuilder
    {
        return CombineWith(decorator, Generate.HashCode64(additionalChecksum));
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="int"/> value containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
    /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static T CombineWith<T>(this IDecorator<T> decorator, int additionalChecksum) where T : ChecksumBuilder
    {
        return CombineWith(decorator, Convertible.GetBytes(additionalChecksum));
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="long"/> value containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
    /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static T CombineWith<T>(this IDecorator<T> decorator, long additionalChecksum) where T : ChecksumBuilder
    {
        return CombineWith(decorator, Convertible.GetBytes(additionalChecksum));
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="float"/> value containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
    /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static T CombineWith<T>(this IDecorator<T> decorator, float additionalChecksum) where T : ChecksumBuilder
    {
        return CombineWith(decorator, Convertible.GetBytes(additionalChecksum));
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="ushort"/> value containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
    /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static T CombineWith<T>(this IDecorator<T> decorator, ushort additionalChecksum) where T : ChecksumBuilder
    {
        return CombineWith(decorator, Convertible.GetBytes(additionalChecksum));
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="uint"/> value containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
    /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static T CombineWith<T>(this IDecorator<T> decorator, uint additionalChecksum) where T : ChecksumBuilder
    {
        return CombineWith(decorator, Convertible.GetBytes(additionalChecksum));
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="ulong"/> value containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
    /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static T CombineWith<T>(this IDecorator<T> decorator, ulong additionalChecksum) where T : ChecksumBuilder
    {
        return CombineWith(decorator, Convertible.GetBytes(additionalChecksum));
    }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum" /> to the representation of this instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ChecksumBuilder"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="additionalChecksum">A <see cref="byte"/> array containing a checksum of the additional data the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/> must represent.</param>
    /// <returns>An updated instance of the enclosed <see cref="ChecksumBuilder"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static T CombineWith<T>(this IDecorator<T> decorator, byte[] additionalChecksum) where T : ChecksumBuilder
    {
        Validator.ThrowIfNull(decorator);
        if (additionalChecksum == null) { return decorator.Inner; }
        if (additionalChecksum.Length == 0) { return decorator.Inner; }
        decorator.Inner.CombineWith(additionalChecksum);
        return decorator.Inner;
    }
}
