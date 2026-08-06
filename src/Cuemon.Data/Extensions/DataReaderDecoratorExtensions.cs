using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Cuemon.Data;
/// <summary>
/// Extension methods for the <see cref="IDataReader"/> interface hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class DataReaderDecoratorExtensions
{
    /// <summary>
    /// Converts the enclosed <see cref="IDataReader"/> of the <paramref name="decorator"/> to an equivalent <see cref="Stream"/> representation.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A <see cref="Stream"/> that is equivalent to the enclosed <see cref="IDataReader"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> is null or its underlying value is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="decorator"/> underlying value is not valid.
    /// </exception>
    /// <remarks><see cref="IDataReader"/> must return only one field (for instance, an XML field), otherwise an <see cref="ArgumentException"/> is thrown.</remarks>
    public static Stream ToStream(this IDecorator<IDataReader> decorator)
    {
        Validator.ThrowIfNull(decorator, out var reader);
        Validator.ThrowIfTrue(reader.FieldCount > 1, nameof(reader), $"The executed command statement appears to contain invalid fields. Expected field count is 1. Actually field count was {reader.FieldCount}.");
        return Patterns.SafeInvoke(() => new MemoryStream(), ms =>
        {
            while (reader.Read())
            {
                var bytes = Convertible.GetBytes(reader.GetString(0));
                ms.Write(bytes, 0, bytes.Length);
            }
            ms.Position = 0;
            return ms;
        });
    }


    /// <summary>
    /// Converts the enclosed <see cref="IDataReader"/> of the <paramref name="decorator"/> to an equivalent <see cref="string"/> representation.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A <see cref="string"/> value that is equivalent to the enclosed <see cref="IDataReader"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> is null or its underlying value is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="decorator"/> underlying value is not valid.
    /// </exception>
    /// <remarks><see cref="IDataReader"/> must return only one field (for instance, an XML field), otherwise an <see cref="ArgumentException"/> is thrown.</remarks>
    public static string ToEncodedString(this IDecorator<IDataReader> decorator)
    {
        using var binary = ToStream(decorator);
        return new StreamReader(binary).ReadToEnd();
    }

    /// <summary>
    /// Asynchronously converts the enclosed <see cref="IDataReader"/> of the <paramref name="decorator"/> to an equivalent <see cref="string"/> representation.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="string"/> value that is equivalent to the enclosed <see cref="IDataReader"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> is null or its underlying value is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="decorator"/> underlying value is not valid.
    /// </exception>
    /// <remarks><see cref="IDataReader"/> must return only one field (for instance, an XML field), otherwise an <see cref="ArgumentException"/> is thrown.</remarks>
    public static Task<string> ToEncodedStringAsync(this IDecorator<IDataReader> decorator)
    {
        using var binary = ToStream(decorator);
        return new StreamReader(binary).ReadToEndAsync();
    }
}
