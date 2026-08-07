using System;

namespace Cuemon;
/// <summary>
/// Provides a way of invoking an <see cref="Func{TResult}" /> delegate regardless of the amount of parameters provided.
/// </summary>
/// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="TResult">The type of the return value of the function delegate <see cref="Method"/>.</typeparam>
public sealed class FuncFactory<TTuple, TResult> : MutableTupleFactory<TTuple> where TTuple : MutableTuple
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FuncFactory{TTuple,TResult}"/> class.
    /// </summary>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    public FuncFactory(Func<TTuple, TResult> method, TTuple tuple) : this(method, tuple, method)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FuncFactory{TTuple,TResult}"/> class.
    /// </summary>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    /// <param name="originalDelegate">The original delegate wrapped by <paramref name="method"/>.</param>
    public FuncFactory(Func<TTuple, TResult> method, TTuple tuple, Delegate originalDelegate) : base(tuple, originalDelegate != null)
    {
        Method = method;
        DelegateInfo = Decorator.RawEnclose(method).ResolveDelegateInfo(originalDelegate);
    }

    /// <summary>
    /// Gets the function delegate to invoke.
    /// </summary>
    /// <value>The function delegate to invoke.</value>
    private Func<TTuple, TResult> Method { get; }

    /// <summary>
    /// Executes the function delegate associated with this instance.
    /// </summary>
    /// <returns>The result of the function delegate associated with this instance.</returns>
    public TResult ExecuteMethod()
    {
        ThrowIfNoValidDelegate(Condition.IsNull(Method));
        return Method(GenericArguments);
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="FuncFactory{TTuple,TResult}"/> object.
    /// </summary>
    /// <returns>A new <see cref="FuncFactory{TTuple,TResult}"/> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTupleFactory<TTuple> Clone()
    {
        return new FuncFactory<TTuple, TResult>(Method, GenericArguments.Clone() as TTuple);
    }
}
