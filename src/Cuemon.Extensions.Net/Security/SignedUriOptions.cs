using System;
using Cuemon.Configuration;
using Cuemon.Security.Cryptography;

namespace Cuemon.Extensions.Net.Security;
/// <summary>
/// Configuration options for <see cref="StringExtensions.ToSignedUri"/> and <see cref="UriExtensions.ToSignedUri"/>.
/// </summary>
public class SignedUriOptions : IParameterObject
{
    private string _contentMd5Header;
    private Func<string, string> _canonicalRepresentationBuilder;
    private string _signatureFieldName;
    private string _startFieldName;
    private string _expiryFieldName;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignedUriOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="SignedUriOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Algorithm"/></term>
    ///         <description><see cref="KeyedCryptoAlgorithm.HmacSha256"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="CanonicalRepresentationBuilder"/></term>
    ///         <description><c>uriString => HasContentMd5Header ? string.Join("\n", ContentMd5Header, uriString) : uriString;</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ContentMd5Header"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="SignatureFieldName"/></term>
    ///         <description>sig</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="StartFieldName"/></term>
    ///         <description>st</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ExpiryFieldName"/></term>
    ///         <description>se</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public SignedUriOptions()
    {
        Algorithm = KeyedCryptoAlgorithm.HmacSha256;
        CanonicalRepresentationBuilder = uriString => HasContentMd5Header ? string.Join("\n", ContentMd5Header, uriString) : uriString;
        SignatureFieldName = "sig";
        StartFieldName = "st";
        ExpiryFieldName = "se";
    }

    /// <summary>
    /// Gets or sets a value indicating whether the signed URI should be URL encoded.
    /// </summary>
    /// <value><c>true</c> if signed URI should be URL encoded; otherwise, <c>false</c>.</value>
    public bool UrlEncode { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that produces the output of a canonical message representation.
    /// </summary>
    /// <value>The function delegate that produces the output of a canonical message representation.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public Func<string, string> CanonicalRepresentationBuilder
    {
        get => _canonicalRepresentationBuilder;
        set
        {
            Validator.ThrowIfNull(value);
            _canonicalRepresentationBuilder = value;
        }
    }

    /// <summary>
    /// Gets or sets the <see cref="KeyedCryptoAlgorithm"/> that defines the HMAC cryptographic implementation.
    /// </summary>
    /// <value>The <see cref="KeyedCryptoAlgorithm"/> that defines the HMAC cryptographic implementation.</value>
    public KeyedCryptoAlgorithm Algorithm { get; set; }

    /// <summary>
    /// Gets or sets the name of the signature field in the querystring. Default is "sig".
    /// </summary>
    /// <value>The name of the signature field in the querystring.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public string SignatureFieldName
    {
        get => _signatureFieldName;
        set
        {
            Validator.ThrowIfNullOrWhitespace(value);
            _signatureFieldName = value;
        }
    }

    /// <summary>
    /// Gets or sets the name of the start field in the querystring. Default is "st".
    /// </summary>
    /// <value>The name of the start field in the querystring.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public string StartFieldName
    {
        get => _startFieldName;
        set
        {
            Validator.ThrowIfNullOrWhitespace(value);
            _startFieldName = value;
        }
    }

    /// <summary>
    /// Gets or sets the name of the expiry field in the querystring. Default is "se".
    /// </summary>
    /// <value>The name of the expiry field in the querystring.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public string ExpiryFieldName
    {
        get => _expiryFieldName;
        set
        {
            Validator.ThrowIfNullOrWhitespace(value);
            _expiryFieldName = value;
        }
    }

    /// <summary>
    /// Gets a value indicating whether this instance has a Content-MD5 header representation.
    /// </summary>
    /// <value><c>true</c> if this instance has a Content-MD5 header representation; otherwise, <c>false</c>.</value>
    public bool HasContentMd5Header => !string.IsNullOrWhiteSpace(ContentMd5Header);

    /// <summary>
    /// Gets or sets the Content-MD5 header representation.
    /// </summary>
    /// <value>The Content-MD5 header representation.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> must consist only of base-64 digits - or -
    /// <paramref name="value"/> has a length that is lower than 128-bit / 16 byte - or -
    /// <paramref name="value"/> has a length that is greater than 128-bit / 16 byte. 
    /// </exception>
    public string ContentMd5Header
    {
        get => _contentMd5Header;
        set
        {
            Validator.ThrowIfNullOrWhitespace(value);
            Validator.ThrowIfLowerThan(value.Length, MessageDigest5.BitSize / Convertible.BitsPerNibble, nameof(value), FormattableString.Invariant($"The length of the value is lower than 128-bit / 32 byte."));
            Validator.ThrowIfGreaterThan(value.Length, MessageDigest5.BitSize / Convertible.BitsPerNibble, nameof(value), FormattableString.Invariant($"The length of the value is greater than 128-bit / 32 byte."));
            Validator.ThrowIfNotBase64String(value);
            _contentMd5Header = value;
        }
    }
}
