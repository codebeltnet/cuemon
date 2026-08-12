using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading;
/// <summary>
/// Provides a way of invoking an <see cref="Func{TResult}" /> function delegate regardless of the amount of parameters provided.
/// </summary>
/// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="TResult">The type of the return value of the function delegate <see cref="Method"/>.</typeparam>
public sealed class AsyncFuncFactory<TTuple, TResult> : MutableTupleFactory<TTuple> where TTuple : MutableTuple
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncFuncFactory{TTuple,TResult}"/> class.
    /// </summary>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    public AsyncFuncFactory(Func<TTuple, CancellationToken, Task<TResult>> method, TTuple tuple) : this(method, tuple, method)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncFuncFactory{TTuple,TResult}"/> class.
    /// </summary>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    /// <param name="originalDelegate">The original delegate wrapped by <paramref name="method"/>.</param>
    public AsyncFuncFactory(Func<TTuple, CancellationToken, Task<TResult>> method, TTuple tuple, Delegate originalDelegate) : base(tuple, originalDelegate != null)
    {
        Method = method;
        DelegateInfo = Decorator.RawEnclose(method).ResolveDelegateInfo(originalDelegate);
    }

    /// <summary>
    /// Gets the function delegate to invoke.
    /// </summary>
    /// <value>The function delegate to invoke.</value>
    private Func<TTuple, CancellationToken, Task<TResult>> Method { get; set; }

    /// <summary>
    /// Executes the function delegate associated with this instance.
    /// </summary>
    /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate associated with this instance.</returns>
    /// <exception cref="InvalidOperationException">
    /// No delegate was specified on the factory.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The <paramref name="ct"/> was canceled.
    /// </exception>
    public Task<TResult> ExecuteMethodAsync(CancellationToken ct)
    {
        ThrowIfNoValidDelegate(Condition.IsNull(Method));
        ct.ThrowIfCancellationRequested();
        return Method.Invoke(GenericArguments, ct);
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="AsyncFuncFactory{TTuple,TResult}"/> object.
    /// </summary>
    /// <returns>A new <see cref="AsyncFuncFactory{TTuple,TResult}"/> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTupleFactory<TTuple> Clone()
    {
        return new AsyncFuncFactory<TTuple, TResult>(Method, GenericArguments.Clone() as TTuple);
    }
}
