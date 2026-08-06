using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading;
/// <summary>
/// Provides a way of invoking an <see cref="Action" /> delegate regardless of the amount of parameters provided.
/// </summary>
/// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="MutableTuple"/>.</typeparam>
public sealed class AsyncActionFactory<TTuple> : MutableTupleFactory<TTuple> where TTuple : MutableTuple
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncActionFactory{TTuple}"/> class.
    /// </summary>
    /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    public AsyncActionFactory(Func<TTuple, CancellationToken, Task> method, TTuple tuple) : this(method, tuple, method)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncActionFactory{TTuple}"/> class.
    /// </summary>
    /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    /// <param name="originalDelegate">The original delegate wrapped by <paramref name="method"/>.</param>
    public AsyncActionFactory(Func<TTuple, CancellationToken, Task> method, TTuple tuple, Delegate originalDelegate) : base(tuple, originalDelegate != null)
    {
        Method = method;
        DelegateInfo = Decorator.RawEnclose(method).ResolveDelegateInfo(originalDelegate);
    }

    /// <summary>
    /// Gets the delegate to invoke.
    /// </summary>
    /// <value>The delegate to invoke.</value>
    private Func<TTuple, CancellationToken, Task> Method { get; }

    /// <summary>
    /// Executes the delegate associated with this instance.
    /// </summary>
    /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">
    /// No delegate was specified on the factory.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The <paramref name="ct"/> was canceled.
    /// </exception>
    public Task ExecuteMethodAsync(CancellationToken ct)
    {
        ThrowIfNoValidDelegate(Condition.IsNull(Method));
        ct.ThrowIfCancellationRequested();
        return Method.Invoke(GenericArguments, ct);
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="AsyncActionFactory{TTuple}"/> object.
    /// </summary>
    /// <returns>A new <see cref="AsyncActionFactory{TTuple}"/> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTupleFactory<TTuple> Clone()
    {
        return new AsyncActionFactory<TTuple>(Method, GenericArguments.Clone() as TTuple);
    }
}
