using System;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Reflection;
/// <summary>
/// Extension methods for the <see cref="PropertyInfo"/> class.
/// </summary>
public static class PropertyInfoExtensions
{
    /// <summary>
    /// Determines whether the specified <paramref name="property"/> is considered an automatic property implementation.
    /// </summary>
    /// <param name="property">The property to check for automatic property implementation.</param>
    /// <returns><c>true</c> if the specified <paramref name="property"/> is considered an automatic property implementation; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="property"/> is null.
    /// </exception>
    public static bool IsAutoProperty(this PropertyInfo property)
    {
        Validator.ThrowIfNull(property);
        return Decorator.Enclose(property).IsAutoProperty();
    }
}
