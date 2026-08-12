using System;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Reflection;
/// <summary>
/// Extension methods for the <see cref="MemberInfo"/>.
/// </summary>
public static class MemberInfoExtensions
{
    /// <summary>
    /// Determines whether the specified source type contains one or more of the specified attribute target types.
    /// </summary>
    /// <param name="source">The member to match against.</param>
    /// <param name="targets">The attribute target types to be matched against.</param>
    /// <returns>
    /// 	<c>true</c> if the specified member contains one or more of the specified attribute target types; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasAttributes(this MemberInfo source, params Type[] targets)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(targets);
        return Decorator.Enclose(source).HasAttribute(targets);
    }
}
