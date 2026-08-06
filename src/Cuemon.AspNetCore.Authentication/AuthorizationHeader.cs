using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.AspNetCore.Authentication;
/// <summary>
/// Represents the base class from which all implementations of authorization header should derive.
/// </summary>
public abstract class AuthorizationHeader
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationHeader"/> class.
    /// </summary>
    /// <param name="authenticationScheme">The name of the authentication scheme.</param>
    protected AuthorizationHeader(string authenticationScheme)
    {
        Validator.ThrowIfNullOrWhitespace(authenticationScheme);
        AuthenticationScheme = authenticationScheme;
    }

    /// <summary>
    /// Gets the name of the authentication scheme.
    /// </summary>
    /// <value>The name of the authentication scheme.</value>
    public string AuthenticationScheme { get; }

    /// <summary>
    /// Parses the specified <paramref name="authorizationHeader"/>.
    /// </summary>
    /// <param name="authorizationHeader">The authorization header to parse.</param>
    /// <param name="setup">The <see cref="AuthorizationHeaderOptions" /> which need to be configured.</param>
    /// <returns>An <see cref="AuthorizationHeader"/> equivalent of <paramref name="authorizationHeader"/>.</returns>
    public virtual AuthorizationHeader Parse(string authorizationHeader, Action<AuthorizationHeaderOptions> setup)
    {
        Validator.ThrowIfNullOrWhitespace(authorizationHeader);
        Validator.ThrowIfFalse(() => authorizationHeader.StartsWith(AuthenticationScheme, StringComparison.OrdinalIgnoreCase), nameof(authorizationHeader), $"Header did not start with {AuthenticationScheme}.");
        Validator.ThrowIfInvalidConfigurator(setup, out var options);

        var headerWithoutScheme = authorizationHeader.Remove(0, AuthenticationScheme.Length + 1);
        var credentials = DelimitedString.Split(headerWithoutScheme, o => o.Delimiter = options.CredentialsDelimiter).ToList();

        var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var credential in credentials)
        {
            var kvp = DelimitedString.Split(credential, o => o.Delimiter = options.CredentialsKeyValueDelimiter);
            var key = kvp[0].Trim();
            var value = kvp[1].Trim('"');
            Decorator.Enclose(dictionary).TryAdd(key, value);
        }

        return ParseCore(dictionary);
    }

    /// <summary>
    /// The core parser that resolves an <see cref="AuthorizationHeader"/> from a set of <paramref name="credentials"/>.
    /// </summary>
    /// <param name="credentials">The credentials used in authentication.</param>
    /// <returns>An <see cref="AuthorizationHeader"/> equivalent of <paramref name="credentials"/>.</returns>
    protected abstract AuthorizationHeader ParseCore(IReadOnlyDictionary<string, string> credentials);

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public abstract override string ToString();
}
