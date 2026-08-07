using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;
using Cuemon.Configuration;

namespace Cuemon.Text;
/// <summary>
/// Configuration options for concrete implementations of <see cref="IConfigurableParser{TOptions}"/> that is related to parsing a <see cref="Uri"/> from a <see cref="string"/>.
/// </summary>
/// <seealso cref="IValidatableParameterObject"/>.
public class UriStringOptions : IValidatableParameterObject
{
    /// <summary>
    /// Gets all supported URI schemes.
    /// </summary>
    /// <returns>A sequence of all supported URI schemes.</returns>
    public static IEnumerable<UriScheme> AllUriSchemes => Arguments.ToEnumerableOf(UriScheme.File, UriScheme.Ftp, UriScheme.Gopher, UriScheme.Http, UriScheme.Https, UriScheme.Mailto, UriScheme.NetPipe, UriScheme.NetTcp, UriScheme.News, UriScheme.Nntp);

    /// <summary>
    /// Initializes a new instance of the <see cref="UriStringOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="UriStringOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Kind"/></term>
    ///         <description><see cref="UriKind.Absolute"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Schemes"/></term>
    ///         <description><see cref="AllUriSchemes"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public UriStringOptions()
    {
        Kind = UriKind.Absolute;
        Schemes = new List<UriScheme>(AllUriSchemes);
    }

    /// <summary>
    /// Gets or sets the kind of the URI.
    /// </summary>
    /// <value>The kind of the URI.</value>
    public UriKind Kind { get; set; }

    /// <summary>
    /// Gets or sets a collection of <see cref="UriScheme"/> values that determines the outcome when parsing a URI.
    /// </summary>
    /// <value>The <see cref="UriScheme"/> values that determines the outcome when parsing a URI.</value>
    public IList<UriScheme> Schemes { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="Schemes"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(Schemes == null);
    }
}
