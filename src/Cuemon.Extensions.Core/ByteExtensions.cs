using System;
using System.Text;
using Cuemon.Text;

namespace Cuemon.Extensions;
/// <summary>
/// Extension methods for the <see cref="byte"/> struct.
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    /// Converts the specified <paramref name="bytes"/> to a string using the provided preferred encoding.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/> array to extend.</param>
    /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
    /// <returns>A <see cref="string"/> containing the results of decoding the specified sequence of bytes.</returns>
    /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
    public static string ToEncodedString(this byte[] bytes, Action<EncodingOptions> setup = null)
    {
        return Convertible.ToString(bytes, setup);
    }

    /// <summary>
    /// Converts the specified <paramref name="bytes"/> to its equivalent hexadecimal representation.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/> array to extend.</param>
    /// <returns>A hexadecimal <see cref="string"/> representation of the elements in <paramref name="bytes"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> is null.
    /// </exception>
    public static string ToHexadecimalString(this byte[] bytes)
    {
        return StringFactory.CreateHexadecimal(bytes);
    }

    /// <summary>
    /// Converts the specified <paramref name="bytes"/> to its equivalent binary representation.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/> array to extend.</param>
    /// <returns>A binary <see cref="string"/> representation of the elements in <paramref name="bytes"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> is null.
    /// </exception>
    public static string ToBinaryString(this byte[] bytes)
    {
        return StringFactory.CreateBinaryDigits(bytes);
    }

    /// <summary>
    /// Encodes a byte array into its equivalent string representation using base 64 digits, which is usable for transmission on the URL.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/> array to extend.</param>
    /// <returns>The string containing the encoded token if the byte array length is greater than one; otherwise, an empty string ("").</returns>
    public static string ToUrlEncodedBase64String(this byte[] bytes)
    {
        return StringFactory.CreateUrlEncodedBase64(bytes);
    }

    /// <summary>
    /// Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/> array to extend.</param>
    /// <returns>The string representation, in base 64, of the contents of <paramref name="bytes"/>.</returns>
    public static string ToBase64String(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="byte"/> array.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/> array to extend.</param>
    /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="bytes"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="bytes"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
    /// <returns><c>true</c> if the <paramref name="bytes"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
    public static bool TryDetectUnicodeEncoding(this byte[] bytes, out Encoding result)
    {
        return ByteOrderMark.TryDetectEncoding(bytes, out result);
    }
}
