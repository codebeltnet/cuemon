using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static Task<IReadOnlyCollection<TResult>> WhileResultAsync<TReader, TElement, TResult>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, CancellationToken, Task<TResult>> worker, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileResultCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncFuncFactory.Create(worker, default), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static Task<IReadOnlyCollection<TResult>> WhileResultAsync<TReader, TElement, T, TResult>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, T, CancellationToken, Task<TResult>> worker, T arg, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileResultCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncFuncFactory.Create(worker, default, arg), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static Task<IReadOnlyCollection<TResult>> WhileResultAsync<TReader, TElement, T1, T2, TResult>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileResultCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncFuncFactory.Create(worker, default, arg1, arg2), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static Task<IReadOnlyCollection<TResult>> WhileResultAsync<TReader, TElement, T1, T2, T3, TResult>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, T3, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileResultCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncFuncFactory.Create(worker, default, arg1, arg2, arg3), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static Task<IReadOnlyCollection<TResult>> WhileResultAsync<TReader, TElement, T1, T2, T3, T4, TResult>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, T3, T4, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileResultCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncFuncFactory.Create(worker, default, arg1, arg2, arg3, arg4), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same order as the while loop evaluates <c>true</c>.</returns>
    public static Task<IReadOnlyCollection<TResult>> WhileResultAsync<TReader, TElement, T1, T2, T3, T4, T5, TResult>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Func<TElement, T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(condition);
        Validator.ThrowIfNull(provider);
        Validator.ThrowIfNull(worker);
        return WhileResultCoreAsync(new AsyncForwardIterator<TReader, TElement>(reader, condition, provider), AsyncFuncFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5), setup);
    }

    private static async Task<IReadOnlyCollection<TResult>> WhileResultCoreAsync<TReader, TElement, TWorker, TResult>(AsyncForwardIterator<TReader, TElement> iterator, AsyncFuncFactory<TWorker, TResult> workerFactory, Action<AsyncWorkloadOptions> setup)
        where TWorker : MutableTuple<TElement>
    {
        var options = Patterns.Configure(setup);
        var result = new ConcurrentDictionary<long, TResult>();
        var readForward = true;
        long sorter = 0;

        while (true)
        {
            var workChunks = options.PartitionSize;
            var queue = new Dictionary<long, Task<TResult>>();
            while (workChunks > 0 && readForward)
            {
                readForward = await iterator.ReadAsync().ConfigureAwait(false);
                if (!readForward) { break; }
                workerFactory.GenericArguments.Arg1 = iterator.Current;
                queue.Add(sorter, workerFactory.ExecuteMethodAsync(options.CancellationToken));
                workChunks--;
                sorter++;
            }
            if (queue.Count == 0) { break; }
            await Task.WhenAll(queue.Values).ConfigureAwait(false);
            foreach (var item in queue) { result.TryAdd(item.Key, item.Value.Result); }
        }
        return new ReadOnlyCollection<TResult>(result.Values.ToList());
    }
}
