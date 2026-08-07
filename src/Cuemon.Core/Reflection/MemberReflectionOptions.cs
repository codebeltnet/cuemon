using Cuemon.Configuration;

namespace Cuemon.Reflection;
/// <summary>
/// Configuration options for <see cref="MemberReflection"/>.
/// </summary>
/// <seealso cref="IParameterObject"/>
public class MemberReflectionOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberReflectionOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="MemberReflectionOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="ExcludeInheritancePath"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ExcludePrivate"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ExcludeStatic"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ExcludePublic"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public MemberReflectionOptions()
    {
        ExcludePrivate = false;
        ExcludeStatic = false;
        ExcludeInheritancePath = false;
        ExcludePublic = false;
    }

    /// <summary>
    /// Gets or sets a value indicating whether static members are excluded from the binding constraint.
    /// </summary>
    /// <value><c>true</c> if static members are excluded from the binding constraint; otherwise, <c>false</c>.</value>
    public bool ExcludeStatic { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether non-public members are excluded from the binding constraint.
    /// </summary>
    /// <value><c>true</c> if non-public members are excluded from the binding constraint; otherwise, <c>false</c>.</value>
    public bool ExcludePrivate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether derived members of a type's inheritance path are excluded from the binding constraint.
    /// </summary>
    /// <value><c>true</c> if derived members of a type's inheritance path are excluded from the binding constraint; otherwise, <c>false</c>.</value>
    public bool ExcludeInheritancePath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether public members are excluded from the binding constraint.
    /// </summary>
    /// <value><c>true</c> if public members are excluded from the binding constraint; otherwise, <c>false</c>.</value>
    public bool ExcludePublic { get; set; }
}
