using System;
using Cuemon.Configuration;

namespace Cuemon.AspNetCore.Authentication;
/// <summary>
/// Configuration options for <see cref="AuthorizationHeader"/>.
/// </summary>
public class AuthorizationHeaderOptions : IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationHeaderOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AuthorizationHeaderOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="CredentialsDelimiter"/></term>
    ///         <description>,</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="CredentialsKeyValueDelimiter"/></term>
    ///         <description>=</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AuthorizationHeaderOptions()
    {
        CredentialsDelimiter = ",";
        CredentialsKeyValueDelimiter = "=";
    }

    /// <summary>
    /// Gets or sets the credentials delimiter.
    /// </summary>
    /// <value>The credentials delimiter.</value>
    public string CredentialsDelimiter { get; set; }

    /// <summary>
    /// Gets or sets the credentials key value delimiter.
    /// </summary>
    /// <value>The credentials key value delimiter.</value>
    public string CredentialsKeyValueDelimiter { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <seealso cref="CredentialsDelimiter"/> cannot be null - or -
    /// <seealso cref="CredentialsKeyValueDelimiter"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(CredentialsDelimiter == null);
        Validator.ThrowIfInvalidState(CredentialsKeyValueDelimiter == null);
    }
}
