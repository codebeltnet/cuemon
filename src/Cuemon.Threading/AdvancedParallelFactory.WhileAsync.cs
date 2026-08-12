using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading;
public static partial class AdvancedParallelFactory
{
    /// <summary>
    /// Executes a parallel while loop.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition" /> evaluates <c>true</c>.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
    public static Task WhileAsync<TReader, TElement>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, CancellationToken, Task> worker, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncActionFactory.Create(worker, default), setup);
    }

    /// <summary>
    /// Executes a parallel while loop.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
    /// <param name="arg">The parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task WhileAsync<TReader, TElement, T>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, T, CancellationToken, Task> worker, T arg, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncActionFactory.Create(worker, default, arg), setup);
    }

    /// <summary>
    /// Executes a parallel while loop.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task WhileAsync<TReader, TElement, T1, T2>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, CancellationToken, Task> worker, T1 arg1, T2 arg2, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncActionFactory.Create(worker, default, arg1, arg2), setup);
    }

    /// <summary>
    /// Executes a parallel while loop.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task WhileAsync<TReader, TElement, T1, T2, T3>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, T3, CancellationToken, Task> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncActionFactory.Create(worker, default, arg1, arg2, arg3), setup);
    }

    /// <summary>
    /// Executes a parallel while loop.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task WhileAsync<TReader, TElement, T1, T2, T3, T4>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, T3, T4, CancellationToken, Task> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncActionFactory.Create(worker, default, arg1, arg2, arg3, arg4), setup);
    }

    /// <summary>
    /// Executes a parallel while loop.
    /// </summary>
    /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
    /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="worker"/>.</typeparam>
    /// <param name="reader">The reader that provides forward-only access to data.</param>
    /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
    /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
    /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="worker"/>.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task WhileAsync<TReader, TElement, T1, T2, T3, T4, T5>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, T3, T4, T5, CancellationToken, Task> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncActionFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5), setup);
    }

    private static async Task WhileCoreAsync<TReader, TElement, TWorker>(AsyncForwardIterator<TReader, TElement> iterator, AsyncActionFactory<TWorker> workerFactory, Action<AsyncWorkloadOptions> setup)
        where TWorker : MutableTuple<TElement>
    {
        var options = Patterns.Configure(setup);
        var readForward = true;

        while (true)
        {
            var workChunks = options.PartitionSize;
            var queue = new List<Task>();
            while (workChunks > 0 && readForward)
            {
                readForward = await iterator.ReadAsync().ConfigureAwait(false);
                if (!readForward) { break; }
                workerFactory.GenericArguments.Arg1 = iterator.Current;
                queue.Add(workerFactory.ExecuteMethodAsync(options.CancellationToken));
                workChunks--;
            }
            if (queue.Count == 0) { break; }
            await Task.WhenAll(queue).ConfigureAwait(false);
        }
    }
}
