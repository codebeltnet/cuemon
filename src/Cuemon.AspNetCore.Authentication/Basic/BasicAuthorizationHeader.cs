using System;
using System.Collections.Generic;
using System.Text;
using Cuemon.Net.Http;
using Cuemon.Text;

namespace Cuemon.AspNetCore.Authentication.Basic;
/// <summary>
/// Provides a representation of a HTTP Basic Authentication header.
/// Implements the <see cref="AuthorizationHeader" />
/// </summary>
/// <seealso cref="AuthorizationHeader" />
public class BasicAuthorizationHeader : AuthorizationHeader
{
    /// <summary>
    /// Creates an instance of <see cref="BasicAuthorizationHeader"/> from the specified parameters.
    /// </summary>
    /// <param name="authorizationHeader">The raw HTTP authorization header.</param>
    /// <returns>An instance of <see cref="BasicAuthorizationHeader"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="authorizationHeader"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="authorizationHeader"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public static BasicAuthorizationHeader Create(string authorizationHeader)
    {
        Validator.ThrowIfNullOrWhitespace(authorizationHeader);
        return new BasicAuthorizationHeader().Parse(authorizationHeader, null) as BasicAuthorizationHeader;
    }

    /// <summary>
    /// The default authentication scheme of the <see cref="BasicAuthorizationHeader"/>.
    /// </summary>
    public const string Scheme = HttpAuthenticationSchemes.Basic;

    private BasicAuthorizationHeader() : base(Scheme)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicAuthorizationHeader"/> class.
    /// </summary>
    /// <param name="username">The username of the credentials.</param>
    /// <param name="password">The password of the credentials.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="username"/> cannot be null -or-
    /// <paramref name="password"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="username"/> cannot be empty or consist only of white-space characters -or-
    /// <paramref name="password"/> cannot be empty or consist only of white-space characters -or-
    /// <paramref name="username"/> does not allow the presence of the colon character.
    /// </exception>
    public BasicAuthorizationHeader(string username, string password) : base(Scheme)
    {
        Validator.ThrowIfNullOrWhitespace(username);
        Validator.ThrowIfNullOrWhitespace(password);
        Validator.ThrowIfTrue(() => username.Contains(":"), nameof(username), $"Colon is not allowed as part of the {nameof(username)}.");
        UserName = username;
        Password = password;
    }


    /// <summary>
    /// Gets the username of the credentials.
    /// </summary>
    /// <value>The username of the credentials.</value>
    public string UserName { get; }

    /// <summary>
    /// Gets the password of the credentials.
    /// </summary>
    /// <value>The password of the credentials.</value>
    public string Password { get; }

    private static readonly char[] ColonSeparator = new[] { ':' };

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        var credentials = Convert.ToBase64String(Decorator.Enclose($"{UserName}:{Password}").ToByteArray());
        return $"{AuthenticationScheme} {credentials}";
    }

    /// <summary>
    /// Parses the specified <paramref name="authorizationHeader" />.
    /// </summary>
    /// <param name="authorizationHeader">The authorization header to parse.</param>
    /// <param name="setup">The <see cref="AuthorizationHeaderOptions" /> which need to be configured.</param>
    /// <returns>An <see cref="AuthorizationHeader" /> equivalent of <paramref name="authorizationHeader" />.</returns>
    public override AuthorizationHeader Parse(string authorizationHeader, Action<AuthorizationHeaderOptions> setup)
    {
        Validator.ThrowIfNullOrWhitespace(authorizationHeader);
        Validator.ThrowIfFalse(() => authorizationHeader.StartsWith(AuthenticationScheme, StringComparison.OrdinalIgnoreCase), nameof(authorizationHeader), $"Header did not start with {AuthenticationScheme}.");

        var headerWithoutScheme = authorizationHeader.Remove(0, AuthenticationScheme.Length + 1);
        var credentials = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { BasicFields.Credentials, headerWithoutScheme.Trim() }
        };
        return ParseCore(credentials);
    }

    /// <summary>
    /// The core parser that resolves an <see cref="AuthorizationHeader" /> from a set of <paramref name="credentials" />.
    /// </summary>
    /// <param name="credentials">The credentials used in authentication.</param>
    /// <returns>An <see cref="AuthorizationHeader" /> equivalent of <paramref name="credentials" />.</returns>
    protected override AuthorizationHeader ParseCore(IReadOnlyDictionary<string, string> credentials)
    {
        if (credentials.TryGetValue(BasicFields.Credentials, out var base64EncodedCredentials) && Condition.IsBase64(base64EncodedCredentials))
        {
            var plainCredentials = Convertible.ToString(Convert.FromBase64String(base64EncodedCredentials), options =>
            {
                options.Encoding = Encoding.ASCII;
                options.Preamble = PreambleSequence.Remove;
            }).Split(ColonSeparator, 2);

            if (plainCredentials.Length == 2 &&
                !string.IsNullOrWhiteSpace(plainCredentials[0]) &&
                !string.IsNullOrWhiteSpace(plainCredentials[1]))
            {
                return new BasicAuthorizationHeader(plainCredentials[0], plainCredentials[1]);
            }
        }
        return null;
    }
}
