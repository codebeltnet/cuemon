using System;
using System.Reflection;

namespace Cuemon.Extensions;
/// <summary>
/// Provides a generic way to wrap an object instance of <typeparamref name="T"/> inside another object.
/// </summary>
/// <typeparam name="T">The type of the object instance to wrap inside another object.</typeparam>
public interface IWrapper<out T> : IData
{
    /// <summary>
    /// Gets the instance of the <see cref="IWrapper{T}"/> object.
    /// </summary>
    /// <value>The instance of the <see cref="IWrapper{T}"/> object.</value>
    T Instance { get; }

    /// <summary>
    /// Gets the type of the <see cref="Instance"/>.
    /// </summary>
    /// <value>The type of the <see cref="Instance"/>.</value>
    Type InstanceType { get; }

    /// <summary>
    /// Gets the member from where <see cref="Instance"/> was referenced.
    /// </summary>
    /// <value>The member from where <see cref="Instance"/> was referenced.</value>
    MemberInfo MemberReference { get; }

    /// <summary>
    /// Gets a value indicating whether this instance has a member reference.
    /// </summary>
    /// <value><c>true</c> if this instance has a member reference; otherwise, <c>false</c>.</value>
    bool HasMemberReference { get; }

    /// <summary>
    /// Returns a value that is equivalent to the instance of the node that this hierarchical structure represents.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <returns>A value that is equivalent to the instance of the node that this hierarchical structure represents.</returns>
    TResult InstanceAs<TResult>();

    /// <summary>
    /// Returns a value that is equivalent to the instance of the node that this hierarchical structure represents.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A value that is equivalent to the instance of the node that this hierarchical structure represents.</returns>
    TResult InstanceAs<TResult>(IFormatProvider provider);
}
