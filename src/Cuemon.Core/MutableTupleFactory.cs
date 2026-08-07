using System;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon;
/// <summary>
/// Provides a base-class for delegate based factories.
/// </summary>
/// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="MutableTuple"/>.</typeparam>
public abstract class MutableTupleFactory<TTuple> where TTuple : MutableTuple
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTupleFactory{TTuple}"/> class.
    /// </summary>
    /// <param name="tuple">The n-tuple representation of a <see cref="MutableTuple"/>.</param>
    /// <param name="hasDelegate"><c>true</c> if this instance has a valid delegate; otherwise, <c>false</c>.</param>
    protected MutableTupleFactory(TTuple tuple, bool hasDelegate)
    {
        Validator.ThrowIfNull(tuple);
        GenericArguments = tuple;
        HasDelegate = hasDelegate;
    }

    /// <summary>
    /// Gets a n-tuple representation of a <see cref="MutableTuple"/> that represents the generic arguments passed to this instance.
    /// </summary>
    /// <value>The n-tuple representation of a <see cref="MutableTuple"/> that represents the generic arguments passed to this instance.</value>
    public TTuple GenericArguments { get; }

    /// <summary>
    /// Gets a value indicating whether this instance has an assigned delegate.
    /// </summary>
    /// <value><c>true</c> if this instance an assigned delegate; otherwise, <c>false</c>.</value>
    public virtual bool HasDelegate { get; }

    /// <summary>
    /// Gets the method represented by the delegate.
    /// </summary>
    /// <value>A <see cref="MethodInfo"/> describing the method represented by the delegate. </value>
    public virtual MethodInfo DelegateInfo { get; protected set; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        if (HasDelegate)
        {
            var descriptor = new MethodDescriptor(DelegateInfo);
            return descriptor.ToString();
        }
        return base.ToString();
    }

    /// <summary>
    /// Validates and throws an <see cref="InvalidOperationException"/> if this instance has no valid delegate.
    /// </summary>
    /// <param name="delegateIsNull">The value of a condition that can be either <c>true</c> or <c>false</c>.</param>
    /// <exception cref="InvalidOperationException">
    /// No delegate was specified on the factory.
    /// </exception>
    protected void ThrowIfNoValidDelegate(bool delegateIsNull)
    {
        if (!HasDelegate) { throw new InvalidOperationException(delegateIsNull ? "There is no delegate specified on the factory." : FormattableString.Invariant($"There is a delegate specified on the factory, '{Decorator.Enclose(GetType()).ToFriendlyName(o => o.FullName = true)}', but it leads to a null referenced delegate wrapper.")); }
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTupleFactory{TTuple}"/> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTupleFactory{TTuple}"/> implementation that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public abstract MutableTupleFactory<TTuple> Clone();
}
