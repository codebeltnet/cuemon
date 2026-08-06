using System;
using Cuemon.Text;

namespace Cuemon.Extensions.Text;
/// <summary>
/// Extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Encodes all the characters in the specified <paramref name="value"/> to its encoded <see cref="string"/> variant.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="setup">The <see cref="FallbackEncodingOptions"/> which may be configured.</param>
    /// <returns>A <see cref="string"/> variant of <paramref name="value"/> that is encoded with <see cref="FallbackEncodingOptions.TargetEncoding"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    /// <remarks>The inspiration for this method was retrieved @ SO: https://stackoverflow.com/a/135473/175073.</remarks>
    public static string ToEncodedString(this string value, Action<FallbackEncodingOptions> setup = null)
    {
        Validator.ThrowIfNull(value);
        return Decorator.Enclose(value).ToEncodedString(setup);
    }

    /// <summary>
    /// Encodes all the characters in the specified <paramref name="value"/> to its ASCII encoded variant.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
    /// <returns>A <see cref="string"/> variant of <paramref name="value"/> that is ASCII encoded.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public static string ToAsciiEncodedString(this string value, Action<EncodingOptions> setup = null)
    {
        Validator.ThrowIfNull(value);
        return Decorator.Enclose(value).ToAsciiEncodedString(setup);
    }
}
