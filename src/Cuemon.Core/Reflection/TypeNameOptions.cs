using System;
using System.Globalization;

namespace Cuemon.Reflection;
/// <summary>
/// Configuration options for <see cref="TypeDecoratorExtensions.ToFriendlyName" />.
/// </summary>
public sealed class TypeNameOptions : FormattingOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="TypeNameOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="ExcludeGenericArguments"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="FullName"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="FormattingOptions.FormatProvider"/></term>
    ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="FriendlyNameStringConverter"/></term>
    ///         <description><code>(type, provider, fullname) =>
    ///{
    ///    var typeName = fullname ? type.FullName?.ToString(provider) ?? type.Name.ToString(provider) : type.Name.ToString(provider);
    ///    var indexOfGraveAccent = typeName.IndexOf('`');
    ///    return indexOfGraveAccent >= 0 ? typeName.Remove(indexOfGraveAccent) : typeName;
    ///};</code></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public TypeNameOptions()
    {
        ExcludeGenericArguments = false;
        FullName = false;
        FriendlyNameStringConverter = (type, provider, fullname) =>
        {
            var typeName = fullname ? type.FullName?.ToString(provider) ?? type.Name.ToString(provider) : type.Name.ToString(provider);
            var indexOfGraveAccent = typeName.IndexOf('`');
            return indexOfGraveAccent >= 0 ? typeName.Remove(indexOfGraveAccent) : typeName;
        };
    }

    /// <summary>
    /// Gets or sets a value indicating whether to exclude generic arguments from a <see cref="Type"/>.
    /// </summary>
    /// <value><c>true</c> to exclude generic arguments from a <see cref="Type"/>; otherwise, <c>false</c>.</value>
    public bool ExcludeGenericArguments { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use the fully qualified name of a <see cref="Type"/>.
    /// </summary>
    /// <value><c>true</c> to use the fully qualified name of a <see cref="Type"/>; otherwise, <c>false</c>.</value>
    public bool FullName { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that convert a <see cref="Type"/> object into a human-readable string.
    /// </summary>
    /// <value>The function delegate that convert a <see cref="Type"/> object into a human-readable string.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public Func<Type, IFormatProvider, bool, string> FriendlyNameStringConverter { get; set; }

    /// <inheritdoc />
    public override void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(FriendlyNameStringConverter == null);
        base.ValidateOptions();
    }
}
