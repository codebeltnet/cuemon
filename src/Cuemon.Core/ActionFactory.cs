using System;

namespace Cuemon;
/// <summary>
/// Provides a way of invoking an <see cref="Action" /> delegate regardless of the amount of parameters provided.
/// </summary>
/// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="MutableTuple"/>.</typeparam>
public sealed class ActionFactory<TTuple> : MutableTupleFactory<TTuple> where TTuple : MutableTuple
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionFactory{TTuple}"/> class.
    /// </summary>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    public ActionFactory(Action<TTuple> method, TTuple tuple) : this(method, tuple, method)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionFactory{TTuple}"/> class.
    /// </summary>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    /// <param name="originalDelegate">The original delegate wrapped by <paramref name="method"/>.</param>
    public ActionFactory(Action<TTuple> method, TTuple tuple, Delegate originalDelegate) : base(tuple, originalDelegate != null)
    {
        Method = method;
        DelegateInfo = Decorator.RawEnclose(method).ResolveDelegateInfo(originalDelegate);
    }

    /// <summary>
    /// Gets the delegate to invoke.
    /// </summary>
    /// <value>The delegate to invoke.</value>
    private Action<TTuple> Method { get; set; }

    /// <summary>
    /// Executes the delegate associated with this instance.
    /// </summary>
    public void ExecuteMethod()
    {
        ThrowIfNoValidDelegate(Condition.IsNull(Method));
        Method(GenericArguments);
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="ActionFactory{TTuple}"/> object.
    /// </summary>
    /// <returns>A new <see cref="ActionFactory{TTuple}"/> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTupleFactory<TTuple> Clone()
    {
        return new ActionFactory<TTuple>(Method, GenericArguments.Clone() as TTuple);
    }
}
