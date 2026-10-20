using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cuemon.Text;

namespace Cuemon;
/// <summary>
/// Provides access to factory methods for creating encoded string representations.
/// </summary>
public static class StringFactory
{
    private static readonly IDictionary<UriScheme, string> UriSchemeToStringLookupTable = ParserFactory.StringToUriSchemeLookupTable.ToDictionary(pair => pair.Value, pair => pair.Key);

    /// <summary>
    /// Creates a hexadecimal string representation of the specified byte array.
    /// </summary>
    /// <param name="value">The byte array to convert.</param>
    /// <returns>A hexadecimal string representation of <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static string CreateHexadecimal(byte[] value)
    {
        Validator.ThrowIfNull(value);
#if NET10_0_OR_GREATER
        return Convert.ToHexString(value).Replace("-", "").ToLowerInvariant();
#else
        return BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
#endif
    }

    /// <summary>
    /// Creates a hexadecimal string representation of the specified string.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="setup">The delegate that configures the encoding behavior.</param>
    /// <returns>A hexadecimal string representation of <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="setup"/> configures an invalid value for <see cref="EncodingOptions.Preamble"/>.
    /// </exception>
    public static string CreateHexadecimal(string value, Action<EncodingOptions> setup = null)
    {
        Validator.ThrowIfNull(value);
        var encodedString = Convertible.GetBytes(value, setup);
        return CreateHexadecimal(encodedString);
    }

    /// <summary>
    /// Creates a binary digit string representation of the specified byte array.
    /// </summary>
    /// <param name="value">The byte array to convert.</param>
    /// <returns>A binary digit string representation of <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static string CreateBinaryDigits(byte[] value)
    {
        Validator.ThrowIfNull(value);
        return string.Concat(value.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
    }

    /// <summary>
    /// Creates a URL-safe Base64 string representation of the specified byte array.
    /// </summary>
    /// <param name="value">The byte array to convert.</param>
    /// <returns>A URL-safe Base64 string representation of <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This method uses the Base64 URL encoding convention by removing padding characters and replacing
    /// <c>+</c> with <c>-</c> and <c>/</c> with <c>_</c>.
    /// <para>
    /// The implementation was inspired by Appendix C of the JSON Web Signature (JWS) draft specification.
    /// </para>
    /// </remarks>
    public static string CreateUrlEncodedBase64(byte[] value)
    {
        Validator.ThrowIfNull(value);
        var base64 = Convert.ToBase64String(value);
        base64 = base64.Split('=')[0];
        base64 = base64.Replace('+', '-');
        base64 = base64.Replace('/', '_');
        return base64;
    }

    /// <summary>
    /// Creates a protocol-relative URL string representation of the specified <see cref="Uri"/>.
    /// </summary>
    /// <param name="value">The URI to convert.</param>
    /// <param name="setup">The delegate that configures the protocol-relative URL format.</param>
    /// <returns>A protocol-relative URL string representation of <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is not an absolute URI.
    /// </exception>
    public static string CreateProtocolRelativeUrl(Uri value, Action<ProtocolRelativeUriStringOptions> setup = null)
    {
        Validator.ThrowIfNull(value);
        Validator.ThrowIfFalse(value.IsAbsoluteUri, nameof(value), "Uri must be absolute.");
        var options = Patterns.Configure(setup);
        var schemeLength = value.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.Unescaped).Length;
        return FormattableString.Invariant($"{options.RelativeReference}{value.OriginalString.Remove(0, schemeLength)}");
    }

    /// <summary>
    /// Creates the string representation of the specified <see cref="UriScheme"/>.
    /// </summary>
    /// <param name="value">The URI scheme to convert.</param>
    /// <returns>The string representation of <paramref name="value"/>.</returns>
    /// <remarks>
    /// Returns the string representation of <see cref="UriScheme.Undefined"/> when <paramref name="value"/>
    /// is not found in the lookup table.
    /// </remarks>
    public static string CreateUriScheme(UriScheme value)
    {
        if (!UriSchemeToStringLookupTable.TryGetValue(value, out var result))
        {
            result = UriScheme.Undefined.ToString();
        }
        return result;
    }
}
