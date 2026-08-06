using System;
using System.Reflection;

namespace Cuemon;
/// <summary>
/// Extension methods for the <see cref="Delegate"/> hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class DelegateDecoratorExtensions
{
    /// <summary>
    /// Resolves the <see cref="MethodInfo"/> of the specified <paramref name="original"/> delegate or the enclosed <see cref="Delegate"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="original">The original delegate to resolve the method information from.</param>
    /// <returns>The <see cref="MethodInfo"/> of the specified <paramref name="original"/> delegate or the enclosed <see cref="Delegate"/> of the <paramref name="decorator"/>; otherwise, <c>null</c> if both are <c>null</c>.</returns>
    public static MethodInfo ResolveDelegateInfo(this IDecorator<Delegate> decorator, Delegate original)
    {
        var wrapper = decorator?.Inner;
        if (original != null) { return original.GetMethodInfo(); }
        if (wrapper != null) { return wrapper.GetMethodInfo(); }
        return null;
    }
}
