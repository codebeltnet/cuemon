using System;
using System.Collections.Generic;

namespace Cuemon.Threading;
public static partial class AdvancedParallelFactory
{
    /// <summary>
    /// Executes a parallel while loop where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition" /> evaluates <c>true</c>.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions" /> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static IReadOnlyCollection<TResult> WhileResult<TReader, TElement, TResult>(TReader reader, Func<bool> condition, Func<TReader, TElement> provider, Func<TElement, TResult> worker, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TElement>, TResult>(tuple => worker(tuple.Arg1), new MutableTuple<TElement>(default), worker);
        return WhileResultCore(new ForwardIterator<TReader, TElement>(reader, condition, provider), factory, setup);
    }

    /// <summary>
    /// Executes a parallel while loop where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
    /// <param name="arg">The parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static IReadOnlyCollection<TResult> WhileResult<TReader, TElement, T, TResult>(TReader reader, Func<bool> condition, Func<TReader, TElement> provider, Func<TElement, T, TResult> worker, T arg, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TElement, T>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2), new MutableTuple<TElement, T>(default, arg), worker);
        return WhileResultCore(new ForwardIterator<TReader, TElement>(reader, condition, provider), factory, setup);
    }

    /// <summary>
    /// Executes a parallel while loop where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static IReadOnlyCollection<TResult> WhileResult<TReader, TElement, T1, T2, TResult>(TReader reader, Func<bool> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, TResult> worker, T1 arg1, T2 arg2, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TElement, T1, T2>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<TElement, T1, T2>(default, arg1, arg2), worker);
        return WhileResultCore(new ForwardIterator<TReader, TElement>(reader, condition, provider), factory, setup);
    }

    /// <summary>
    /// Executes a parallel while loop where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static IReadOnlyCollection<TResult> WhileResult<TReader, TElement, T1, T2, T3, TResult>(TReader reader, Func<bool> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, T3, TResult> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TElement, T1, T2, T3>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<TElement, T1, T2, T3>(default, arg1, arg2, arg3), worker);
        return WhileResultCore(new ForwardIterator<TReader, TElement>(reader, condition, provider), factory, setup);
    }

    /// <summary>
    /// Executes a parallel while loop where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static IReadOnlyCollection<TResult> WhileResult<TReader, TElement, T1, T2, T3, T4, TResult>(TReader reader, Func<bool> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, T3, T4, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TElement, T1, T2, T3, T4>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<TElement, T1, T2, T3, T4>(default, arg1, arg2, arg3, arg4), worker);
        return WhileResultCore(new ForwardIterator<TReader, TElement>(reader, condition, provider), factory, setup);
    }

    /// <summary>
    /// Executes a parallel while loop where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static IReadOnlyCollection<TResult> WhileResult<TReader, TElement, T1, T2, T3, T4, T5, TResult>(TReader reader, Func<bool> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, T3, T4, T5, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TElement, T1, T2, T3, T4, T5>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), new MutableTuple<TElement, T1, T2, T3, T4, T5>(default, arg1, arg2, arg3, arg4, arg5), worker);
        return WhileResultCore(new ForwardIterator<TReader, TElement>(reader, condition, provider), factory, setup);
    }

    private static IReadOnlyCollection<TResult> WhileResultCore<TReader, TElement, TWorker, TResult>(ForwardIterator<TReader, TElement> iterator, FuncFactory<TWorker, TResult> workerFactory, Action<AsyncTaskFactoryOptions> setup)
        where TWorker : MutableTuple<TElement>
    {
        return new FuncWhileSynchronousLoop<TReader, TElement, TResult>(iterator, setup).GetResult(workerFactory);
    }
}
