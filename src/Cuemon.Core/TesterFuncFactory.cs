using System;
using System.Reflection;

namespace Cuemon;
/// <summary>
/// Provides a way of invoking an <see cref="TesterFunc{TResult, TSuccess}" /> function delegate regardless of the amount of parameters provided.
/// </summary>
/// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="TResult">The type of the out result value of the tester function delegate <see cref="Method"/>.</typeparam>
/// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <see cref="Method"/>.</typeparam>
public class TesterFuncFactory<TTuple, TResult, TSuccess> : MutableTupleFactory<TTuple> where TTuple : MutableTuple
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TesterFuncFactory{TTuple,TResult,TSuccess}"/> class.
    /// </summary>
    /// <param name="method">The tester function delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    public TesterFuncFactory(TesterFunc<TTuple, TResult, TSuccess> method, TTuple tuple) : this(method, tuple, method)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TesterFuncFactory{TTuple,TResult,TSuccess}"/> class.
    /// </summary>
    /// <param name="method">The tester function delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    /// <param name="originalDelegate">The original delegate wrapped by <paramref name="method"/>.</param>
    public TesterFuncFactory(TesterFunc<TTuple, TResult, TSuccess> method, TTuple tuple, Delegate originalDelegate) : base(tuple, originalDelegate != null)
    {
        Method = method;
        DelegateInfo = Decorator.RawEnclose(method).ResolveDelegateInfo(originalDelegate);
    }

    /// <summary>
    /// Gets the tester function delegate to invoke.
    /// </summary>
    /// <value>The <see cref="TesterFunc{TResult, TSuccess}"/> delegate to invoke.</value>
    protected TesterFunc<TTuple, TResult, TSuccess> Method { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance has an assigned tester function delegate.
    /// </summary>
    /// <value><c>true</c> if this instance an assigned tester function delegate; otherwise, <c>false</c>.</value>
    public override bool HasDelegate => base.HasDelegate;

    /// <summary>
    /// Gets the method represented by the tester function delegate.
    /// </summary>
    /// <value>A <see cref="MethodInfo" /> describing the method represented by the tester function delegate.</value>
    public sealed override MethodInfo DelegateInfo => base.DelegateInfo;

    /// <summary>
    /// Executes the tester function delegate associated with this instance.
    /// </summary>
    /// <param name="result">The out result value of the tester function delegate.</param>
    /// <returns>The return value that indicates success of the tester function delegate associated with this instance.</returns>
    public virtual TSuccess ExecuteMethod(out TResult result)
    {
        return Method(GenericArguments, out result);
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="TesterFuncFactory{TTuple,TResult,TSuccess}"/> object.
    /// </summary>
    /// <returns>A new <see cref="TesterFuncFactory{TTuple,TResult,TSuccess}"/> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTupleFactory<TTuple> Clone()
    {
        return new TesterFuncFactory<TTuple, TResult, TSuccess>(Method, GenericArguments.Clone() as TTuple);
    }
}
