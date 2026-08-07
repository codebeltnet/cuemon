using System;
using System.Reflection;

namespace Cuemon.Reflection;
/// <summary>
/// Extension methods for the <see cref="MethodInfo"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class MethodInfoDecoratorExtensions
{
    /// <summary>
    /// Determines whether the underlying <see cref="MethodInfo"/> of the <paramref name="decorator"/> has been overridden.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the underlying <see cref="MethodInfo"/> of the <paramref name="decorator"/> has been overridden; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsOverridden(this IDecorator<MethodInfo> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return decorator.Inner.GetBaseDefinition().DeclaringType != decorator.Inner.DeclaringType;
    }
}
