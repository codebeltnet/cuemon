using System;
using System.Globalization;
using System.Text;
using Cuemon.Security.Cryptography;
using Cuemon.Text;

namespace Cuemon.AspNetCore.Authentication.Digest;
/// <summary>
/// Configuration options for <see cref="DigestAuthenticationMiddleware"/>. This class cannot be inherited.
/// </summary>
/// <seealso cref="AuthenticationOptions" />
public sealed class DigestAuthenticationOptions : AuthenticationOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DigestAuthenticationOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="DigestAuthenticationOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Authenticator"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="DigestAlgorithm"/></term>
    ///         <description><see cref="DigestCryptoAlgorithm.Sha256"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="NonceGenerator"/></term>
    ///         <description>A default implementation of a nonce generator.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="OpaqueGenerator"/></term>
    ///         <description>A default implementation of an opaque generator.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="NonceExpiredParser"/></term>
    ///         <description>A default implementation of a nonce expiry parser.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="NonceSecret"/></term>
    ///         <description>A default secret to get you started without overwhelming configuration. Do change when moving outside a development environment.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Realm"/></term>
    ///         <description>AuthenticationServer</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="UseServerSideHa1Storage"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public DigestAuthenticationOptions()
    {
        DigestAlgorithm = DigestCryptoAlgorithm.Sha256;
        OpaqueGenerator = () => Generate.RandomString(32, Alphanumeric.Hexadecimal).ToLowerInvariant();
        NonceExpiredParser = (nonce, timeToLive) =>
        {
            Validator.ThrowIfNullOrEmpty(nonce);
            if (ParserFactory.FromBase64().TryParse(nonce, out var rawNonce))
            {
                var nonceProtocol = Convertible.ToString(rawNonce, options =>
                {
                    options.Encoding = Encoding.UTF8;
                    options.Preamble = PreambleSequence.Remove;
                });
                var nonceTimestamp = DateTime.ParseExact(nonceProtocol.Substring(0, nonceProtocol.LastIndexOf(':')), "u", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                var difference = (DateTime.UtcNow - nonceTimestamp);
                return (difference > timeToLive);
            }
            return false;
        };
        NonceGenerator = (timestamp, entityTag, privateKey) =>
        {
            Validator.ThrowIfNullOrWhitespace(entityTag);
            Validator.ThrowIfNull(privateKey);
            var nonceHash = UnkeyedHashFactory.CreateCryptoSha256().ComputeHash(timestamp.Ticks, entityTag, Convert.ToBase64String(privateKey)).ToHexadecimalString();
            var nonceProtocol = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", timestamp.ToString("u", CultureInfo.InvariantCulture), nonceHash);
            return Convert.ToBase64String(Convertible.GetBytes(nonceProtocol, options =>
            {
                options.Encoding = Encoding.UTF8;
                options.Preamble = PreambleSequence.Remove;
            }));
        };
        NonceSecret = () => Convert.FromBase64String("ZHBGWDRrVGVxbFlhVEpWQ3hoYUc5VUlZM05penNOaUk=");
        Realm = "AuthenticationServer";
    }

    /// <summary>
    /// Gets or sets the function delegate that will perform the authentication from the specified <c>username</c>.
    /// </summary>
    /// <value>The function delegate that will perform the authentication.</value>
    public DigestAuthenticator Authenticator { get; set; }

    /// <summary>
    /// Specifies the cryptographic algorithm used in HTTP Digest Access Authentication. Default is <see cref="DigestCryptoAlgorithm.Sha256"/>.
    /// </summary>
    public DigestCryptoAlgorithm DigestAlgorithm { get; set; }

    /// <summary>
    /// Gets the realm that defines the protection space.
    /// </summary>
    /// <value>The realm that defines the protection space.</value>
    public string Realm { get; set; }

    /// <summary>
    /// Gets or sets the function delegate for generating opaque string values.
    /// </summary>
    /// <value>The function delegate for generating opaque string values.</value>
    public Func<string> OpaqueGenerator { get; set; }

    /// <summary>
    /// Gets or sets the function delegate for retrieving the cryptographic secret used in nonce string values.
    /// </summary>
    /// <value>The function delegate for retrieving the cryptographic secret used in nonce string values.</value>
    public Func<byte[]> NonceSecret { get; set; }

    /// <summary>
    /// Gets or sets the function delegate for generating nonce string values.
    /// </summary>
    /// <value>The function delegate for generating nonce string values.</value>
    public Func<DateTime, string, byte[], string> NonceGenerator { get; set; }

    /// <summary>
    /// Gets or sets the function delegate for parsing nonce string values for expiration.
    /// </summary>
    /// <value>The function delegate for parsing nonce string values for expiration.</value>
    public Func<string, TimeSpan, bool> NonceExpiredParser { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the server should bypass the calculation of HA1 password representation.
    /// </summary>
    /// <value><c>true</c> if the server should bypass the calculation of HA1 password representation; otherwise, <c>false</c>.</value>
    /// <remarks>When enabled, the server reads the HA1 value directly from a secured storage, hence this cannot be used in combination with session variants of <see cref="DigestCryptoAlgorithm"/>.</remarks>
    public bool UseServerSideHa1Storage { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <seealso cref="Authenticator"/> cannot be null - or -
    /// <seealso cref="NonceExpiredParser"/> cannot be null - or -
    /// <seealso cref="NonceGenerator"/> cannot be null - or -
    /// <seealso cref="NonceSecret"/> cannot be null - or -
    /// <seealso cref="OpaqueGenerator"/> cannot be null - or -
    /// <seealso cref="Realm"/> cannot be null, empty or consist only of white-space characters.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public override void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(Authenticator == null);
        Validator.ThrowIfInvalidState(string.IsNullOrWhiteSpace(Realm));
        Validator.ThrowIfInvalidState(NonceExpiredParser == null);
        Validator.ThrowIfInvalidState(NonceGenerator == null);
        Validator.ThrowIfInvalidState(NonceSecret == null);
        Validator.ThrowIfInvalidState(OpaqueGenerator == null);
        base.ValidateOptions();
    }
}
