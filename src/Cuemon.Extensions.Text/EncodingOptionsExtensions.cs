using System.IO;
using System.Text;
using Cuemon.Text;

namespace Cuemon.Extensions.Text;
/// <summary>
/// Extension methods for the <see cref="IEncodingOptions"/> interface.
/// </summary>
public static class EncodingOptionsExtensions
{
    /// <summary>
    /// Tries to detect an <see cref="Encoding"/> object from the specified <paramref name="stream"/>.
    /// If unsuccessful, the <see cref="EncodingOptions.Encoding"/> value is returned.
    /// </summary>
    /// <param name="options">The <see cref="IEncodingOptions"/> to extend.</param>
    /// <param name="stream">The <see cref="Stream"/> to parse for an <see cref="Encoding"/>.</param>
    /// <returns>Either the detected encoding of <paramref name="stream"/>  or the encoding of this instance.</returns>
    public static Encoding DetectUnicodeEncoding(this IEncodingOptions options, Stream stream)
    {
        Validator.ThrowIfNull(options);
        return ByteOrderMark.DetectEncodingOrDefault(stream, options.Encoding);
    }

    /// <summary>
    /// Tries to detect an <see cref="Encoding"/> object from the specified <paramref name="bytes"/>.
    /// If unsuccessful, the <see cref="EncodingOptions.Encoding"/> value is returned.
    /// </summary>
    /// <param name="options">The <see cref="IEncodingOptions"/> to extend.</param>
    /// <param name="bytes">The <see cref="byte"/> array to parse for an <see cref="Encoding"/>.</param>
    /// <returns>Either the detected encoding of <paramref name="bytes"/> or the encoding of this instance.</returns>
    public static Encoding DetectUnicodeEncoding(this IEncodingOptions options, byte[] bytes)
    {
        Validator.ThrowIfNull(options);
        return ByteOrderMark.DetectEncodingOrDefault(bytes, options.Encoding);
    }
}
