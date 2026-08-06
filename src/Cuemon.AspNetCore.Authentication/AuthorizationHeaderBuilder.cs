using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.AspNetCore.Authentication;
/// <summary>
/// Represents the base class from which all implementations of authorization header builders should derive.
/// Implements the <see cref="AuthorizationHeaderBuilder" />
/// </summary>
/// <typeparam name="TAuthorizationHeader">The type of the authorization header result.</typeparam>
/// <typeparam name="TAuthorizationHeaderBuilder">The type of the authorization header builder.</typeparam>
public abstract class AuthorizationHeaderBuilder<TAuthorizationHeader, TAuthorizationHeaderBuilder> : AuthorizationHeaderBuilder
    where TAuthorizationHeader : AuthorizationHeader
    where TAuthorizationHeaderBuilder : AuthorizationHeaderBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationHeaderBuilder{TAuthorizationHeader,TAuthorizationHeaderBuilder}"/> class.
    /// </summary>
    /// <param name="authenticationScheme">The name of the authentication scheme.</param>
    protected AuthorizationHeaderBuilder(string authenticationScheme) : base(authenticationScheme)
    {
    }

    /// <summary>
    /// Attempts to add or update an existing field with the provided <paramref name="name"/> with the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="name">The name of the field to add or update.</param>
    /// <param name="value">The value of the field to add or update.</param>
    /// <returns><typeparamref name="TAuthorizationHeaderBuilder"/> that can be used to further build the header.</returns>
    public TAuthorizationHeaderBuilder AddOrUpdate(string name, string value)
    {
        Validator.ThrowIfNullOrWhitespace(name);
        Decorator.Enclose(Data).AddOrUpdate(name, value);
        return this as TAuthorizationHeaderBuilder;
    }

    /// <summary>
    /// Builds an instance of <typeparamref name="TAuthorizationHeader"/> that implements <see cref="AuthorizationHeader"/>.
    /// </summary>
    /// <returns><typeparamref name="TAuthorizationHeader"/>.</returns>
    public abstract TAuthorizationHeader Build();
}

/// <summary>
/// The base class of an <see cref="AuthorizationHeaderBuilder"/>.
/// </summary>
public abstract class AuthorizationHeaderBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationHeaderBuilder"/> class.
    /// </summary>
    /// <param name="authenticationScheme">The name of the authentication scheme.</param>
    protected AuthorizationHeaderBuilder(string authenticationScheme)
    {
        Validator.ThrowIfNullOrWhitespace(authenticationScheme);
        AuthenticationScheme = authenticationScheme;
    }

    /// <summary>
    /// Gets the fields added to this instance.
    /// </summary>
    /// <value>The fields added to this instance.</value>
    protected IDictionary<string, string> Data { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the relations added to this instance.
    /// </summary>
    /// <value>The relations added to this instance.</value>
    protected IDictionary<string, string> Relation { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the name of the authentication scheme.
    /// </summary>
    /// <value>The name of the authentication scheme.</value>
    public string AuthenticationScheme { get; }

    /// <summary>
    /// Maps the logical relation between a <paramref name="memberName"/> and the <paramref name="fieldNames"/> associated.
    /// </summary>
    /// <param name="memberName">The name of a member.</param>
    /// <param name="fieldNames">The field names to associate with <paramref name="memberName"/>.</param>
    protected void MapRelation(string memberName, params string[] fieldNames)
    {
        Validator.ThrowIfNullOrWhitespace(memberName);
        Validator.ThrowIfSequenceNullOrEmpty(fieldNames, nameof(fieldNames));
        foreach (var key in fieldNames) { Decorator.Enclose(Relation).AddOrUpdate(key, memberName); }
    }

    /// <summary>
    /// Validates that any <paramref name="requiredFieldNames"/> has been added to <see cref="Data"/>.
    /// </summary>
    /// <param name="requiredFieldNames">The required field names to validate.</param>
    /// <exception cref="ArgumentException">
    /// The required field is missing.
    /// </exception>
    protected void ValidateData(params string[] requiredFieldNames)
    {
        foreach (var rfn in requiredFieldNames)
        {
            var invalidState = !Data.ContainsKey(rfn) || Data[rfn] == null;
            if (Relation.TryGetValue(rfn, out var member) && invalidState) { throw new ArgumentException($"Required field is missing. Did you forget to invoke {member}?", rfn); }
            if (invalidState) { throw new ArgumentException("Required field is missing.", rfn); }
        }
    }

    /// <summary>
    /// Converts this instance to an <see cref="ImmutableDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <returns>An <see cref="ImmutableDictionary{TKey,TValue}"/> equivalent of this instance.</returns>
    public ImmutableDictionary<string, string> ToImmutableDictionary()
    {
        return Data.ToImmutableDictionary();
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return DelimitedString.Create(Data.Keys.Select(key => $"{key}={Data[key]}"), o => o.Delimiter = Environment.NewLine + Environment.NewLine);
    }
}
